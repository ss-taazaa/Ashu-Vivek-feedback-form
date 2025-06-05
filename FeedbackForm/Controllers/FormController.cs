using FeedbackForm.DTOs;
using FeedbackForm.Models;
using FeedbackForm.Services.Implementations;
using FeedbackForm.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/forms")]
public class FormsController : ControllerBase
{
    private readonly IFormService _formService;
    private readonly IUserService _userService;

    public FormsController(IFormService formService, IUserService userService)
    {
        _formService = formService;
        _userService = userService;
    }

    [HttpPost]
    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CreateForm([FromBody] CreateFormRequestDto request)
    {
        // Step 1: Validate if user exists
        var user = await _userService.GetUserById(request.UserId);
        if (user == null)
            return NotFound($"User with ID {request.UserId} not found.");

        // Step 2: Create form with FK to user
        var form = new Form
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            CreatedOn = DateTime.UtcNow,
            UserId = request.UserId,
            ShareableLink = Guid.NewGuid().ToString(),
            Questions = request.Questions?.Select(q => new Question
            {
                Id = Guid.NewGuid(),
                Text = q.Text,
                Type = q.Type,
                WordLimit = q.WordLimit ?? 0,
                IsRequired = q.IsRequired,
                Order = q.Order,
                Options = q.Options?.Select(o => new Option
                {
                    Id = Guid.NewGuid(),
                    Text = o.Text,
                    Value = o.Value,
                    Order = o.Order
                }).ToList()
            }).ToList()
        };

        var createdForm = await _formService.CreateFormAsync(form);

        return CreatedAtAction(nameof(GetFormById), new { id = createdForm.Id }, createdForm);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetFormById(Guid id)
    {
        var form = await _formService.GetFormByIdAsync(id);
        if (form == null) return NotFound();
        return Ok(form);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllForms()
    {
        var forms = await _formService.GetAllFormsAsync();
        return Ok(forms);
    }

    [HttpPost("{id}/status")]
    public async Task<IActionResult> UpdateFormStatus(Guid id, [FromQuery] FormStatus status)
    {
        bool result = false;

        switch (status)
        {
            case FormStatus.Published:
                result = await _formService.PublishFormAsync(id);
                break;
            case FormStatus.Closed:
                result = await _formService.CloseFormAsync(id);
                break;
            default:
                return BadRequest("Unsupported status update. Only 'Published' or 'Closed' are allowed.");
        }

        return result ? Ok($"Form status updated to '{status}'.") : BadRequest("Unable to update form status.");
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateForm(Guid id, [FromBody] FormUpdateDto dto)
    {
        var form = await _formService.GetFormByIdAsync(id);
        if (form == null) BadRequest("Form ID mismatch.");

        form.Title = dto.Title;
        form.Description = dto.Description;
        form.Status = (FormStatus)dto.Status;
        form.PublishedOn = dto.PublishedOn;
        form.ClosedOn = dto.ClosedOn;
        form.ShareableLink = dto.ShareableLink;

        var updated = await _formService.UpdateFormAsync(form);
        return Ok(updated);
    }



    [HttpPut("{id}/questions")]
    public async Task<IActionResult> UpdateFormQuestions(Guid id, [FromBody] List<Question> questions)
    {
        var result = await _formService.UpdateFormQuestionsAsync(id, questions);
        return result ? Ok("Form questions updated successfully.") : BadRequest("Failed to update form questions.");
    }
}

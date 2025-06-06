﻿using FeedbackForm.DTOs;
using FeedbackForm.Models;
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
    public async Task<IActionResult> CreateForm([FromBody] CreateFormRequestDto request)
    {

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
        if (form == null)
            return NotFound();

        var dto = new FormDetailGetByIdDto
        {
            Id = form.Id,
            Title = form.Title,
            Description = form.Description,
            Status = form.Status.ToString(),
            ShareableLink = form.Status == FormStatus.Published ? form.ShareableLink : null,
            CreatedOn = form.CreatedOn,
            PublishedOn = form.PublishedOn,
            ClosedOn = form.ClosedOn,
            UserId = form.UserId,
            Questions = form.Questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                FormId = q.FormId,
                Text = q.Text,
                Type = q.Type,
                WordLimit = q.WordLimit,
                IsRequired = q.IsRequired,
                Order = q.Order,
                Options = q.Options.Select(o => new OptionDto
                {
                    Id = o.Id,
                    QuestionId = o.QuestionId,
                    Text = o.Text,
                    Value = o.Value,
                    Order = o.Order
                }).ToList()
            }).ToList(),
            Submissions = form.Submissions.Select(s => new SubmissionDto
            {
                Id = s.Id,
                //FormId = s.FormId,
                SubmittedOn = s.SubmittedOn,
                RespondentId = s.RespondentId,
                Answers = s.Answers.Select(a => new AnswerDto
                {
                    Id = a.Id,
                    QuestionId = a.QuestionId,
                    //OptId = a.OptId,
                    TextAnswer = a.TextAnswer
                }).ToList()
            }).ToList()
        };

        return Ok(dto);
    }


    [HttpGet]
    public async Task<IActionResult> GetAllForms()
    {
        var forms = await _formService.GetAllFormsAsync();

        var result = forms.Select(f => new FormListItemDto
        {
            Id = f.Id,
            Title = f.Title,
            Description = f.Description,
            Status = f.Status.ToString(),
            ShareableLink = f.Status == FormStatus.Published ? f.ShareableLink : null,
            //SubmissionCount = (f.Status == FormStatus.Published || f.Status == FormStatus.Closed) ? f.Submissions?.Count ?? 0 : 0,
            PublishedOn = f.PublishedOn,
            ClosedOn = f.ClosedOn,
            CanEdit = f.Status == FormStatus.Draft,
            CanClose = f.Status == FormStatus.Published
        });

        return Ok(result);
    }

    [HttpPut("{id}/status")]
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

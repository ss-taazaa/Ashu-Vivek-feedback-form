using FeedbackForm.DTOs;
using FeedbackForm.Enum;
using FeedbackForm.Helper;
using FeedbackForm.Models;
using FeedbackForm.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/form")]
public class FormsController(IFormService _formService, IUserService _userService) : ControllerBase
{


    [HttpPost]
    public async Task<IActionResult> CreateForm([FromBody] CreateFormRequestDto request)
    {
        var user = await _userService.GetUserById(request.UserId);
        if (user == null)
            return NotFound(new ApiResponseDto { Success = false, Message = "User not found." });
        try
        {
            if (!Utils.ValidateQuestions(request).Success)
                return BadRequest(new ApiResponseDto { Success = false, Message = "Invalid questions format." });
            var form = new Form(request);
            await _formService.CreateFormAsync(form);
            return Ok(new ApiResponseDto { Success = true, Message = "Form created successfully." });
        }
        catch (Exception)
        {
            return BadRequest(new ApiResponseDto { Success = false, Message = "Failed to create form." });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFormById(Guid id)
    {
        var form = await _formService.GetFormByIdAsync(id);
        if (form == null) return NotFound();
        var dto = new FormDto(form);

        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllForms()
    {
        var forms = await _formService.GetAllFormsAsync();
        var result = forms.Select(f => new FormListItemDto(f));
        return Ok(result);
    }



    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateFormStatus(Guid id, [FromQuery] FormStatus status)
    {
        bool result;
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
        var updatedResult = await _formService.EditForm(id, dto);
        if (updatedResult)
        {
            return Ok(updatedResult);
        }
        return BadRequest("Error while updating the form");

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteForm(Guid id)
    {
        try
        {
            var deleted = await _formService.DeleteForm(id);
            if (!deleted)
                return NotFound(new { Message = "Form not found or already deleted." });
            return Ok(new { Message = "Form soft-deleted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
        }
    }










}

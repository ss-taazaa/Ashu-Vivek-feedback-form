using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedbackForm.DTOs;
using FeedbackForm.Services.Interfaces;
using FeedbackForm.Helper;
using FeedbackForm.Services.Implementations;

namespace FeedbackForm.Controllers
{
    [ApiController]
    [Route("api/response")]
    public class ResponseController(IResponseService _responseService) : ControllerBase
    {
       
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitForm([FromBody] SubmitFormRequestDto request)
        {

            if (!Utils.NameValidator(request.RespondentName).Success || !Utils.EmailValidator(request.RespondentEmail).Success)
            {
                return BadRequest("Invalid user data.");
            }
            else if (!!Utils.ShareableLinkValidator(request.ShareableLink).Success)
            {
                return BadRequest("Invalid shareable link");
            }
            try
                {
                    await _responseService.SubmitFormAsync(request);
                    return Ok("Form submitted successfully.");
                }
            catch (Exception ex)
            {
                    return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubmissions()
        {
            var submissions = await _responseService.GetAllSubmissionsAsync();
            return Ok(submissions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubmissionById(Guid id)
        {
            var submission = await _responseService.GetSubmissionByIdAsync(id);
            if (submission == null)
                return NotFound($"Submission with ID {id} not found.");

            return Ok(submission);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubmission(Guid id)
        {
            try
            {


                var deletedSubmission = await _responseService.DeleteSubmission(id);
                if (!deletedSubmission)

                    return NotFound(new { Message = "Submission not found or already deleted." });
                return Ok(new { Message = "Submission soft-deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Error = ex.Message });
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeedbackForm.DTOs;
using FeedbackForm.Services.Interfaces;

namespace FeedbackForm.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _responseService;

        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;
        }
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitForm([FromBody] SubmitFormRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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


    }
}

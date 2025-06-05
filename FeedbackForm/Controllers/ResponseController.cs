//using FeedbackForm.DTOs;
//using FeedbackForm.Services.Interfaces;
//using Microsoft.AspNetCore.Mvc;

//[ApiController]
//[Route("api/response")]
//public class ResponseController : ControllerBase
//{
//    private readonly IResponseService _responseService;

//    public ResponseController(IResponseService responseService)
//    {
//        _responseService = responseService;
//    }

//    [HttpPost("submit")]
//    public async Task<IActionResult> SubmitForm([FromBody] SubmitFormRequestDto dto)
//    {
//        try
//        {
//            await _responseService.SubmitFormAsync(dto);
//            return Ok(new { message = "Form submitted successfully" });
//        }
//        catch (Exception ex)
//        {
//            return BadRequest(new { error = ex.Message });
//        }
//    }
//}

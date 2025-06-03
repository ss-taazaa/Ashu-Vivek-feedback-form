using FeedbackForm.DTOs;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/questions")]
public class QuestionsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<QuestionDto>> GetAllQuestions()
    {
        var questions = new List<QuestionDto>
        {
            new QuestionDto
            {
                Id = Guid.NewGuid(),
                FormId = Guid.NewGuid(),
                Text = "What is your favorite programming language?",
                Type = 1,
                WordLimit = 0,
                IsRequired = true,
                Order = 1,
                Options = new List<OptionDto>
                {
                    new OptionDto { Id = Guid.NewGuid(), QuestionId = Guid.NewGuid(), Text = "C#", Value = 1, Order = 1 },
                    new OptionDto { Id = Guid.NewGuid(), QuestionId = Guid.NewGuid(), Text = "Python", Value = 2, Order = 2 }
                }
            }
        };

        return Ok(questions);
    }

    [HttpGet("{id}")]
    public ActionResult<QuestionDto> GetQuestionById(Guid id)
    {
        var question = new QuestionDto
        {
            Id = id,
            FormId = Guid.NewGuid(),
            Text = "Sample question?",
            Type = 2,
            WordLimit = 100,
            IsRequired = false,
            Order = 2,
            Options = new List<OptionDto>()
        };

        return Ok(question);
    }

    [HttpPost]
    public ActionResult<QuestionDto> CreateQuestion([FromBody] QuestionDto questionDto)
    {
        questionDto.Id = Guid.NewGuid();
        return CreatedAtAction(nameof(GetQuestionById), new { id = questionDto.Id }, questionDto);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateQuestion(Guid id, [FromBody] QuestionDto questionDto)
    {
        if (id != questionDto.Id)
            return BadRequest();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteQuestion(Guid id)
    {
        return NoContent();
    }
}

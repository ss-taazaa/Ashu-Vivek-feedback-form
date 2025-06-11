

using FeedbackForm.DTOs;
using FeedbackForm.Models;

public class AnswerDto
{
    public Guid QuestionId { get; set; }
    public string? QuestionText { get; set; }
    public string? QuestionType { get; set; }
    public string? TextAnswer { get; set; }
    public int? RatingValue { get; set; }
    public int? Ranking { get; set; }

    public List<AnswerOptionDto>? AnswerOptions { get; set; }

    public AnswerDto()
    {

    }
    public AnswerDto(Answer answer)
    {
        QuestionId = answer.QuestionId;
        QuestionText = answer.Question?.Text;
        QuestionType = answer.Question?.Type.ToString();

        TextAnswer = answer.TextAnswer;
        RatingValue = answer.RatingValue;
        Ranking = answer.Ranking;

        AnswerOptions = answer.AnswerOptions?.Select(ao => new AnswerOptionDto
        {
            OptId = ao.OptionId,
            OptionText = ao.Option?.Text
        }).ToList();
    }

}

using FeedbackForm.DTOs;

public class AnswerDto
{
    public Guid QuestionId { get; set; }
    public string? TextAnswer { get; set; }
    public int RatingValue { get; set; }
    public int Ranking { get; set; }
    public List<AnswerOptionDto>? AnswerOptions { get; set; }
}

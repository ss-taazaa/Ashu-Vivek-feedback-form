using FeedbackForm.DTOs;
using FeedbackForm.Models;

public class AnswerOption : BaseEntity
{
    public Guid AnswerId { get; set; }
    public Answer Answer { get; set; }

    public Guid OptionId { get; set; }
    public Option Option { get; set; }

    public int? Rank { get; set; } 
    public AnswerOption() { }

    public AnswerOption(Guid answerId, Guid optionId, int? rank)
    {
        AnswerId = answerId;
        OptionId = optionId;
        Rank = rank;
    }
    public AnswerOption(AnswerOptionDto dto)
    {
        Id = Guid.NewGuid();
        OptionId = dto.OptId;
    }

}

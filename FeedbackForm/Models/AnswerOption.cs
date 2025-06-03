using FeedbackForm.Models;

public class AnswerOption : BaseEntity
{
    public Guid AnswerId { get; set; }
    public Answer Answer { get; set; }

    public Guid OptId { get; set; }
    public Option Option { get; set; }

    public int? Rank { get; set; } 
    public AnswerOption() { }

    public AnswerOption(Guid answerId, Guid optId, int? rank)
    {
        AnswerId = answerId;
        OptId = optId;
        Rank = rank;
    }
}

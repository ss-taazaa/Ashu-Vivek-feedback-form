
using FeedbackForm.DTOs;
using FeedbackForm.Enum;
using FeedbackForm.Models;

public class QuestionDto

{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public QuestionType Type { get; set;}
    public int? WordLimit { get; set;}
    public bool IsRequired { get; set; }
    public int Order { get; set; }
    public List<OptionDto> Options { get; set; }
   
    public QuestionDto(Question question)
    {
        Id = question.Id;
        Text = question.Text;
        Type = question.Type;
        WordLimit = question.WordLimit;
        IsRequired = question.IsRequired;
        Order = question.Order;
        Options = question.Options?.Select(o => new OptionDto(o, question.Type)).ToList();

        if (Type == QuestionType.SingleChoice || Type == QuestionType.MultiChoice)
        {
            foreach (var option in Options)
            {
                option.Value = null;
            }
        }
    }
}

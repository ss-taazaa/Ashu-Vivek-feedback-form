using FeedbackForm.DTOs;
using FeedbackForm.Enum;

public class UpdateQuestionDto
{
    public Guid Id { get; set; }  
    public string Text { get; set; }
    public int? WordLimit { get; set; }
    public bool IsRequired { get; set; }
    public int Order { get; set; }
    public List<CreateOptionDto>? Options { get; set; }
}

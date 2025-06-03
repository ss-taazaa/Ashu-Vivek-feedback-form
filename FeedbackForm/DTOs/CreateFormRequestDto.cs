using FeedbackForm.Enum;
using FeedbackForm.Models;

namespace FeedbackForm.DTOs
{
    public class CreateFormRequestDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public FormStatus Status { get; set; }
        public Guid UserId { get; set; }
        public List<CreateQuestionDto> Questions { get; set; }
    }

    public class CreateQuestionDto
    {
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public int? WordLimit { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; }
        public List<CreateOptionDto>? Options { get; set; }
    }

    public class CreateOptionDto
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public int Order { get; set; }
    }
}

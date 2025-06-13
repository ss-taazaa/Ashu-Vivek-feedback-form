using FeedbackForm.Enum;
using System.ComponentModel.DataAnnotations;

namespace FeedbackForm.DTOs
{
    public class CreateFormRequestDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title can't exceed 200 characters.")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description can't exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [EnumDataType(typeof(FormStatus), ErrorMessage = "Invalid form status.")]
        public FormStatus Status { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "At least one question is required.")]
        [MinLength(1, ErrorMessage = "At least one question is required.")]
        public List<CreateQuestionDto> Questions { get; set; }
    }

    public class CreateQuestionDto 
    {
        [Required(ErrorMessage = "Question text is required.")]
        [StringLength(500, ErrorMessage = "Question text can't exceed 500 characters.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Question type is required.")]
        [EnumDataType(typeof(QuestionType), ErrorMessage = "Invalid question type.")]
        public QuestionType Type { get; set; }

        [Range(0, 500, ErrorMessage = "Word limit must be between 0 and 500.")]
        public int? WordLimit { get; set; }

        [Required(ErrorMessage = "IsRequired must be specified.")]
        public bool IsRequired { get; set; }

        [Range(0, 100, ErrorMessage = "Order must be between 0 and 100.")]
        public int Order { get; set; }

        public List<CreateOptionDto>? Options { get; set; }
    }

    public class CreateOptionDto
    {
        [Required(ErrorMessage = "Option text is required.")]
        [StringLength(200, ErrorMessage = "Option text can't exceed 200 characters.")]
        public string Text { get; set; }

        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public int Value { get; set; }

        [Range(0, 100, ErrorMessage = "Order must be between 0 and 100.")]
        public int Order { get; set; }
    }
}

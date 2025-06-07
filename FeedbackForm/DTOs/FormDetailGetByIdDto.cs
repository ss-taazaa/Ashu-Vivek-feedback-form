namespace FeedbackForm.DTOs
{
    public class FormDetailGetByIdDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? ShareableLink { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? PublishedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public Guid UserId { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
        public List<SubmissionDto> Submissions { get; set; } = new();
    }
}

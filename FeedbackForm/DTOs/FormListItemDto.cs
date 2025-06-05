namespace FeedbackForm.DTOs
{
    public class FormListItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string? ShareableLink { get; set; }
        //public int SubmissionCount { get; set; }
        public DateTime? PublishedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public bool CanEdit { get; set; }
        public bool CanClose { get; set; }
    }
}
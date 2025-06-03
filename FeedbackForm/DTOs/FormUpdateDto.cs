namespace FeedbackForm.DTOs
{
    public class FormUpdateDto
    {
        //public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime? PublishedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public string? ShareableLink { get; set; }
    }
}

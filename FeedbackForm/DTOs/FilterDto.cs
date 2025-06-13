namespace FeedbackForm.DTOs
{
    public class FormFilterDto
    {
        public string? Title { get; set; }
        public int? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

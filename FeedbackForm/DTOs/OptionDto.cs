namespace FeedbackForm.DTOs
{
    public class OptionDto
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public int? Value { get; set; }
        public int Order { get; set; }
    }
}

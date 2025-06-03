namespace FeedbackForm.DTOs
{
    public class QuestionDto
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }
        public int WordLimit { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; }
        public List<OptionDto> Options { get; set; }
    }
}

namespace FeedbackForm.DTOs
{
    public class AnswerOptionDto
    {
        public Guid Id { get; set; }
        public Guid AnswerId { get; set; }
        public Guid OptId { get; set; }
        public int Rank { get; set; }
    }
}

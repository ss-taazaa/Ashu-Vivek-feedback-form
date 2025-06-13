namespace FeedbackForm.Models
{
    public class Option : BaseEntity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public string Text { get; set; }
        public int? Value { get; set; } 
        public int Order { get; set; }

        public ICollection<AnswerOption>? AnswerOptions { get; set; }= new List<AnswerOption>();

        public Option() { }

        public Option(Guid questionId, string text, int? value, int order)
        {
            QuestionId = questionId;
            Text = text;
            Value = value;
            Order = order;
        }
    }
}

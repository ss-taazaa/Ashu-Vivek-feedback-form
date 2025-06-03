namespace FeedbackForm.Models
{
    public class Answer : BaseEntity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public Guid SubmissionId { get; set; }
        public Submission Submission { get; set; }

        public string TextAnswer { get; set; }         // For text or multi-line answers
        public int? RatingValue { get; set; }          // For star rating questions
        public int? Ranking { get; set; }              // For ranking questions

        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();

        public Answer() { }

       
        public Answer(Guid questionId, Guid submissionId, string textAnswer, int? ratingValue, int? ranking)
        {
            QuestionId = questionId;
            SubmissionId = submissionId;
            TextAnswer = textAnswer;
            RatingValue = ratingValue;
            Ranking = ranking;
        }
    }
}

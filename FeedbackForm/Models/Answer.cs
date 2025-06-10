namespace FeedbackForm.Models
{
    public class Answer : BaseEntity
    {
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public Guid SubmissionId { get; set; }
        public Submission Submission { get; set; }

        public string? TextAnswer { get; set; }         
        public int? RatingValue { get; set; }         
        public int? Ranking { get; set; }            

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
        public Answer(AnswerDto dto)
        {
            Id = Guid.NewGuid();
            QuestionId = dto.QuestionId;
            TextAnswer = dto.TextAnswer ?? string.Empty;

            RatingValue = dto.RatingValue;
            Ranking = dto.Ranking;
            AnswerOptions = dto.AnswerOptions?.Select(o => new AnswerOption(o)).ToList() ?? new();
        }

    }
}

namespace FeedbackForm.Models
{
    public class Submission : BaseEntity
    {
        public Guid FormId { get; set; }
        public Form Form { get; set; }

        public DateTime SubmittedOn { get; set; }
        public bool isDeleted { get; set; }
        //public DateTime isModified { get; set; }

        public string RespondentName { get; set; }
        public string RespondentEmail { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

        public Submission() { }

        public Submission(Guid formId, DateTime submittedOn, string respondentName, string respondentEmail)
        {
            FormId = formId;
            SubmittedOn = submittedOn;
            RespondentName = respondentName;
            RespondentEmail = respondentEmail;
        }

        public Submission(SubmitFormRequestDto dto, Guid formId)
        {
            Id = Guid.NewGuid();
            FormId = formId;
            RespondentName = dto.RespondentName;
            RespondentEmail = dto.RespondentEmail;
            SubmittedOn = DateTime.UtcNow;

            Answers = dto.Answers?.Select(dtoAnswer =>
            {
                var answer = new Answer
                {
                    Id = Guid.NewGuid(),
                    QuestionId = dtoAnswer.QuestionId,
                    SubmissionId = Id,
                    TextAnswer = dtoAnswer.TextAnswer,
                    RatingValue = dtoAnswer.RatingValue,
                    Ranking = dtoAnswer.Ranking,
                    AnswerOptions = dtoAnswer.AnswerOptions?.Select(dtoOpt => new AnswerOption(dtoOpt)
                    {
                        AnswerId = Guid.Empty
                    }).ToList() ?? new List<AnswerOption>()
                };

                foreach (var answerOption in answer.AnswerOptions)
                {
                    answerOption.AnswerId = answer.Id;
                }

                return answer;
            }).ToList() ?? new List<Answer>();
        }




    }
}

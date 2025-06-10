
using FeedbackForm.Models;

public class SubmissionDto
{
    public Guid Id { get; set; }
    public string RespondentName { get; set; }
    public string RespondentEmail { get; set; }
    public DateTime SubmittedOn { get; set; }

    public List<AnswerDto> Answers { get; set; }

    public SubmissionDto(Submission submission)
    {
        Id = submission.Id;
        RespondentName = submission.RespondentName;
        RespondentEmail = submission.RespondentEmail;
        SubmittedOn = submission.SubmittedOn;
        Answers = submission.Answers.Select(a => new AnswerDto(a)).ToList();
    }
}

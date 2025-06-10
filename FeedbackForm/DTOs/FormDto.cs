using FeedbackForm.Enum;
using FeedbackForm.Models;

public class FormDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public FormStatus Status { get; set; }
    public string ShareableLink { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? PublishedOn { get; set; }
    public DateTime? ClosedOn { get; set; }
    public List<QuestionDto> Questions { get; set; }
    public List<SubmissionDto> Submissions { get; set; }

    // New property to expose submission count
    public int SubmissionCount => Submissions?.Count ?? 0;

    public FormDto(Form form)
    {
        Id = form.Id;
        Title = form.Title;
        Description = form.Description;
        Status = form.Status;
        ShareableLink = form.ShareableLink;
        CreatedOn = form.CreatedOn;
        PublishedOn = form.PublishedOn;
        ClosedOn = form.ClosedOn;

        Questions = form.Questions?.Select(q => new QuestionDto(q)).ToList();
        Submissions = form.Submissions?.Select(s => new SubmissionDto(s)).ToList();
    }
}

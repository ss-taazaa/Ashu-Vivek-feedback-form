using FeedbackForm.Enum;
using FeedbackForm.Models;

public class FormListItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string? ShareableLink { get; set; }
    public DateTime? PublishedOn { get; set; }
    public DateTime? ClosedOn { get; set; }
    public bool CanEdit { get; set; }
    public bool CanClose { get; set; }

    // Constructor to map from Form entity/model
    public FormListItemDto(Form form)
    {
        Id = form.Id;
        Title = form.Title;
        Description = form.Description;
        Status = form.Status.ToString();
        ShareableLink = form.Status == FormStatus.Published ? form.ShareableLink : null;
        PublishedOn = form.PublishedOn;
        ClosedOn = form.ClosedOn;
        CanEdit = form.Status == FormStatus.Draft;
        CanClose = form.Status == FormStatus.Published;
    }
}

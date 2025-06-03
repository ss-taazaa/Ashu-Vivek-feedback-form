namespace FeedbackForm.Models
{
    public enum FormStatus { Draft, Published, Closed }

    public class Form : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public FormStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? PublishedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public string ShareableLink { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();

        public Form() { }

        public Form(string title, string description, FormStatus status, DateTime createdOn, DateTime? publishedOn, DateTime? closedOn, string shareableLink, Guid userId)
        {
            Title = title;
            Description = description;
            Status = status;
            CreatedOn = createdOn;
            PublishedOn = publishedOn;
            ClosedOn = closedOn;
            ShareableLink = shareableLink;
            UserId = userId;
        }
    }
}

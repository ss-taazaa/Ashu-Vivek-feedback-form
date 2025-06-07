using FeedbackForm.DTOs;
using FeedbackForm.Enum;

namespace FeedbackForm.Models
{
  

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

        public Form(CreateFormRequestDto request)
        {
            Id = Guid.NewGuid();
            Title = request.Title;
            Description = request.Description;
            Status = request.Status;
            CreatedOn = DateTime.UtcNow;
            UserId = request.UserId;
            ShareableLink = Guid.NewGuid().ToString();

            Questions = request.Questions?.Select(q => new Question
            {
                Id = Guid.NewGuid(),
                Text = q.Text,
                Type = q.Type,
                WordLimit = q.WordLimit ?? 0,
                IsRequired = q.IsRequired,
                Order = q.Order,
                Options = q.Options?.Select(o => new Option
                {
                    Id = Guid.NewGuid(),
                    Text = o.Text,
                    Value = o.Value,
                    Order = o.Order
                }).ToList()
            }).ToList();
        }


        public void UpdateFromDto(FormUpdateDto dto)
        {
            Title = dto.Title;
            Description = dto.Description;
            Status = (FormStatus)dto.Status;
            PublishedOn = dto.PublishedOn;
            ClosedOn = dto.ClosedOn;
            ShareableLink = dto.ShareableLink;
        }

    }
}

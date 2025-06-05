using System;
using System.Collections.Generic;

namespace FeedbackForm.Models
{
    public class Submission : BaseEntity
    {
        public Guid FormId { get; set; }
        public Form Form { get; set; }

        public DateTime SubmittedOn { get; set; }

        public string RespondentName { get; set; }         // Replaces RespondentId
        public string RespondentEmail { get; set; }        // Replaces RespondentId

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

        public Submission() { }

        public Submission(Guid formId, DateTime submittedOn, string respondentName, string respondentEmail)
        {
            FormId = formId;
            SubmittedOn = submittedOn;
            RespondentName = respondentName;
            RespondentEmail = respondentEmail;
        }
    }
}

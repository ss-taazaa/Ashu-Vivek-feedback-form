using System;
using System.Collections.Generic;

namespace FeedbackForm.Models
{
    public class Submission : BaseEntity
    {
        public Guid FormId { get; set; }
        public Form Form { get; set; }

        public DateTime SubmittedOn { get; set; }
        public string RespondentId { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

       
        public Submission() { }

        
        public Submission(Guid formId, DateTime submittedOn, string respondentId)
        {
            FormId = formId;
            SubmittedOn = submittedOn;
            RespondentId = respondentId;
        }
    }
}

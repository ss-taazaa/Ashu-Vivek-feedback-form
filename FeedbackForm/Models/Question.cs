using FeedbackForm.Enum;
using System;
using System.Collections.Generic;

namespace FeedbackForm.Models
{
    public class Question : BaseEntity
    {
        public Guid FormId { get; set; }
        public Form Form { get; set; }

        public string Text { get; set; }
        public QuestionType Type { get; set; }

        public int? WordLimit { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; }

        public ICollection<Option> Options { get; set; } = new List<Option>();
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();

        public Question() { }

        public Question(Guid formId, string text, QuestionType type, int? wordLimit, bool isRequired, int order)
        {
            FormId = formId;
            Text = text;
            Type = type;
            WordLimit = wordLimit;
            IsRequired = isRequired;
            Order = order;
        }
    }
}

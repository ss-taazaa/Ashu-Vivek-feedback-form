using System.Text.Json.Serialization;
using FeedbackForm.Enum;
using FeedbackForm.Models;

namespace FeedbackForm.DTOs
{
    public class OptionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Value { get; set; }
        public int Order { get; set; }

        public OptionDto(Option option)

        {
            Id = option.Id;
            Text = option.Text;
            Order = option.Order;

            if (questionType == QuestionType.Rating || questionType == QuestionType.Ranking)
            {
                Value = option.Value;
            }


        }
    }
}

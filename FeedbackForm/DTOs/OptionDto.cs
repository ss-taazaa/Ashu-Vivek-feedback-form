//using FeedbackForm.Models;

//namespace FeedbackForm.DTOs
//{
//    public class OptionDto
//    {
//        public Guid Id { get; set; }
//        public string Text { get; set; }
//        public int? Value { get; set; }
//        public int Order { get; set; }
//        public OptionDto(Option option)
//        {
//            Id = option.Id;
//            Text = option.Text;
//            Value = option.Value;
//            Order = option.Order;
//        }
//    }
//}



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


        public OptionDto(Option option, QuestionType questionType)
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
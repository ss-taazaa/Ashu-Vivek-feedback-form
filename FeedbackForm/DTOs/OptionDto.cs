using FeedbackForm.Models;

namespace FeedbackForm.DTOs
{
    public class OptionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public int? Value { get; set; }
        public int Order { get; set; }
        public OptionDto(Option option)
        {
            Id = option.Id;
            Text = option.Text;
            Value = option.Value;
            Order = option.Order;
        }
    }
}

namespace FeedbackForm.DTOs
{
    public class SubmitFormRequestDto
    {
        public string ShareableLink { get; set; }
        public string RespondentId { get; set; }
        public string RespondentName { get; set; }
        public string RespondentEmail { get; set; }
        public List<AnswerDto> Answers { get; set; }
    }
}

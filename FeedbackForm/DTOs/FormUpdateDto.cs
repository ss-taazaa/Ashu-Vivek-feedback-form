namespace FeedbackForm.DTOs
{

    public class FormUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<UpdateQuestionDto> Questions { get; set; }
    }

}

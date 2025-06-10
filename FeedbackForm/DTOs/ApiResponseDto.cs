namespace FeedbackForm.DTOs
{
    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public ApiResponseDto() { }


        public ApiResponseDto(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

}

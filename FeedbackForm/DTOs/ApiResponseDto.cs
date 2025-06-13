namespace FeedbackForm.DTOs
{
    public class ApiResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ApiResponseDto() { }



        public ApiResponseDto(bool success, string message, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }

    }
}

using FeedbackForm.DTOs;
using FeedbackForm.Enum;

namespace FeedbackForm.Helper
{
    public static  class Utils
    {
        public static ApiResponseDto ValidateQuestions(CreateFormRequestDto request)
        {
            if (request.Questions == null || request.Questions.Count == 0)
                return new ApiResponseDto(false,"No Questions found");

            foreach (var question in request.Questions)
            {
                switch (question.Type)
                {
                    case QuestionType.SingleChoice:
                    case QuestionType.MultiChoice:
                        if (question.Options == null || question.Options.Count < 2)
                            return new ApiResponseDto(false , "Invalid Options");
                        if (question.WordLimit.HasValue && question.WordLimit.Value > 0)
                            return new ApiResponseDto(false, "Choice type questions must not have word limit");
                        break;

                    case QuestionType.Text:
                    case QuestionType.Textarea:
                        if (!question.WordLimit.HasValue || question.WordLimit.Value <= 0 || question.WordLimit.Value > 500)

                            return new ApiResponseDto(false, "Answer text type questions must  have word limit between the 0 and 500");
                        if (question.Options != null && question.Options.Any())
                            return new ApiResponseDto(false, "Answer text -type questions should not have option");
                        break;

                    case QuestionType.Rating:
                    case QuestionType.Ranking:
                        if (question.WordLimit.HasValue && question.WordLimit.Value > 0)
                            return new ApiResponseDto(false, "ranking or rating type questions should not have word limit");
                        break;

                    default:
                        return new ApiResponseDto(false, "Invalid question type");
                }
            }

            return new ApiResponseDto(true, "Form created successfully");
        }

        public static ApiResponseDto EmailValidator(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new ApiResponseDto(false, "Email is required.");

            if (email.Contains("@") && email.Contains("."))
                return new ApiResponseDto(true, "Valid email address.");
            else
                return new ApiResponseDto(false, "Invalid email format.");
        }

        public static ApiResponseDto NameValidator(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length<4)
            {
                return new ApiResponseDto(false, "Username is required");
            }
            else
            {
                return new ApiResponseDto(true, "valid user name");
            }
        }




        public static ApiResponseDto ShareableLinkValidator(string shareableLink)
        {
            if (string.IsNullOrWhiteSpace(shareableLink))
                return new ApiResponseDto(false, "Shareable link is required.");
            if (!Guid.TryParse(shareableLink, out _))
                return new ApiResponseDto(false, "Invalid shareable link format.");
            return new ApiResponseDto(true, "Valid shareable link.");
        }

    }
}

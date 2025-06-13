
using System.ComponentModel.DataAnnotations;

namespace FeedbackForm.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your password.")]
        public string Password { get; set; } = string.Empty;
    }
}

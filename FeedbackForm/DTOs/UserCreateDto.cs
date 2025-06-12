using System.ComponentModel.DataAnnotations;

namespace FeedbackForm.DTOs
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "please enter the name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "please enter the email.")]
        [EmailAddress(ErrorMessage = "please enter the correct email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "please enter the password.")]
        [MinLength(6, ErrorMessage = "Password should be at least 6 characters.")]
        public string Password { get; set; }
    }
}


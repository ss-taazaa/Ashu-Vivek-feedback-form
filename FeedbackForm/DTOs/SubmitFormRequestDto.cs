using System.ComponentModel.DataAnnotations;

public class SubmitFormRequestDto
{
    [Required]
    public string ShareableLink { get; set; } = null!;

    [Required]
    public string RespondentName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string RespondentEmail { get; set; } = null!;

    [Required]
    [MinLength(1, ErrorMessage = "At least one answer is required.")]
    public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
}

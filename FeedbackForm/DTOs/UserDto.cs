using FeedbackForm.Models;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedOn { get; set; }
    public List<Guid> FormIds { get; set; }
    public UserDto() { }
    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        CreatedOn = user.CreatedOn;
    }
}

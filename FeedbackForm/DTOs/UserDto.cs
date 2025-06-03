using System;
using System.Collections.Generic;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedOn { get; set; }
    public List<Guid> FormIds { get; set; }
}

using System;
using System.Collections.Generic;

namespace FeedbackForm.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }

        public ICollection<Form> Forms { get; set; } = new List<Form>();

        public User() { }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
            CreatedOn = DateTime.UtcNow;
        }
    }
}

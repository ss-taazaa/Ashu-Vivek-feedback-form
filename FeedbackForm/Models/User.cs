using System;
using System.Collections.Generic;
using FeedbackForm.DTOs;

namespace FeedbackForm.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool isDeleted { get; set; }
        public DateTime? isModified { get; set; } = null;
        public ICollection<Form> Forms { get; set; } = new List<Form>();

        public User() { }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
            CreatedOn = DateTime.UtcNow;
        }
        

        public User(UserCreateDto dto)
        {
            Id = Guid.NewGuid(); 
            Name = dto.Name;
            Email = dto.Email;
            CreatedOn = DateTime.UtcNow;
        }

    }
}

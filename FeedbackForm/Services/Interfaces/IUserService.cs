using FeedbackForm.DTOs;
using FeedbackForm.Models;

namespace FeedbackForm.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserById(Guid id);
    
        Task<User> UpdateUserAsync(Guid id, User user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<User> RegisterAsync(UserCreateDto dto);
        Task<string?> LoginAsync(string email, string password);
        Task LogoutAsync(Guid userId);




    }
}

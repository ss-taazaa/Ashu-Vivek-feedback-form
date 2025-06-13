using System.Linq.Expressions;
using FeedbackForm.DTOs;
using FeedbackForm.Models;

namespace FeedbackForm.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserById(Guid id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(Guid id, User user);
        Task<ApiResponseDto> DeleteUserAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> RegisterAsync(UserCreateDto dto);
        Task<ApiResponseDto> LoginAsync(string email, string password);
        Task LogoutAsync(Guid userId);



    }
}

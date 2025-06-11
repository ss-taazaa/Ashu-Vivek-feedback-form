using FeedbackForm.Services.Interfaces;
using FeedbackForm.Models;
using FeedbackForm.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace FeedbackForm.Services.Implementations
{
    public class UserService (IGenericRepository<User> _userRepository) : IUserService
    {

       
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var existingUsers= await _userRepository.GetAllAsync();
            return existingUsers.Where(s => !s.isDeleted);
        }


        public async Task<User> GetUserById(Guid id)
        {

            var existing= await _userRepository.GetByIdAsync(id);
            if(existing==null || existing.isDeleted)
            {
                return null;
            }
            return existing;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            user.Id = Guid.NewGuid();
            return await _userRepository.AddAsync(user);

        }

        public async Task<User> UpdateUserAsync(Guid id, User updatedUser)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            await _userRepository.UpdateAsync(existingUser);
            return existingUser;

        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var existingUser= await _userRepository.GetByIdAsync(id);
            if(existingUser == null)
            {
                return false;
            }
            existingUser.isDeleted = true;
            existingUser.isModified = DateTime.UtcNow;
            _userRepository.Remove(existingUser);
            return true;
        }
      

    }
}


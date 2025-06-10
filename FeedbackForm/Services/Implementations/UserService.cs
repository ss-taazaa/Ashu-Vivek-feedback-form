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
            return await _userRepository.GetAllAsync();
        }


        public async Task<User> GetUserById(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
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
            _userRepository.Remove(existingUser);
            return true;
        }
      

    }
}


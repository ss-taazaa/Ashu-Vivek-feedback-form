using FeedbackForm.Services.Interfaces;
using FeedbackForm.Models;
using FeedbackForm.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using FeedbackForm.DTOs;
namespace FeedbackForm.Services.Implementations
{
    public class UserService (IGenericRepository<User> _userRepository,IGenericRepository<Submission> _submission) : IUserService
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

        //public async Task<bool> DeleteUserAsync(Guid id)
        //{
        //    var existingUser= await _userRepository.GetByIdAsync(id);
        //    if(existingUser == null)
        //    {
        //        return false;
        //    }


        //    existingUser.isDeleted = true;
        //    existingUser.isModified = DateTime.UtcNow;
        //    _userRepository.Remove(existingUser);
        //    return true;
        //}


        public async Task<ApiResponseDto> DeleteUserAsync(Guid userId)
        {
            var existingUser = await _userRepository.Query()
                .Include(u => u.Forms)
                    .ThenInclude(f => f.Submissions)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (existingUser == null)
                return new ApiResponseDto(false, "No user exists");
            bool hasActiveForms = existingUser.Forms.Any(f => !f.isDeleted);
            if (hasActiveForms)
                return new ApiResponseDto(false, "Delete all forms first. User will be deleted once all the forms are deleted.");
            foreach (var form in existingUser.Forms)
            {
                foreach (var submission in form.Submissions.ToList())
                {
                    _submission.Remove(submission); // hard delete
                }
            }
            existingUser.isDeleted = true;
            existingUser.isModified = DateTime.UtcNow;

            await _userRepository.SaveChangesAsync();

            return new ApiResponseDto(true, "User and associated submissions have been deleted");
        }




        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetSingleAsync(u => u.Email == email);
        }



    }
}


using FeedbackForm.Services.Interfaces;
using FeedbackForm.Models;
using FeedbackForm.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using FeedbackForm.DTOs;
using FeedbackForm.Helper;
namespace FeedbackForm.Services.Implementations
{
    public class UserService (IGenericRepository<User> _userRepository,IGenericRepository<Submission> _submission, JwtHelper _jwtHelper) : IUserService
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

        public async Task<User> RegisterAsync(UserCreateDto dto)
        {
            var existingUser = await _userRepository.GetQueryable().FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (existingUser != null)
                throw new Exception("User already exists.");

            var user = new User(dto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.isActive = true;
            return await _userRepository.AddAsync(user);
        }



        public async Task<ApiResponseDto> LoginAsync(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetQueryable().FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return null;
                }
                var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (!isPasswordValid)
                {
                    return new ApiResponseDto(false,"Invalid Password");
                }
                var token = _jwtHelper.GenerateToken(user);
                user.isActive = true;
                _userRepository.SaveChanges();
                return new ApiResponseDto(true,"Login successful", new { Token = token });
            }
            catch (Exception ex)
            {
                return new ApiResponseDto(false, $"An error occurred during login: {ex.Message}");
            }
        }




        public async Task LogoutAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.isActive = true;
                await _userRepository.UpdateAsync(user);
            }
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
            bool hasActiveSubmissions = existingUser.Forms
            .SelectMany(f => f.Submissions)
            .Any(s => !s.isDeleted);
            if (hasActiveSubmissions)
                return new ApiResponseDto(false, "Delete all submissions first. User will be deleted once all the submissions are deleted.");
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


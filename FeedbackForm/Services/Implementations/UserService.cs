using FeedbackForm.DTOs;
using FeedbackForm.Helper;
using FeedbackForm.Models;
using FeedbackForm.Repositories.Interfaces;
using FeedbackForm.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace FeedbackForm.Services.Implementations
{
    public class UserService(IGenericRepository<User> _userRepository, JwtHelper _jwtHelper) : IUserService
    {
        public async Task<User> RegisterAsync(UserCreateDto dto)
        {
            var existingUser = await _userRepository.GetQueryable().FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existingUser != null)
                throw new Exception("User already exists.");

            var user = new User(dto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password); 
            user.IsActive = 1; // enforce active

            return await _userRepository.AddAsync(user);
        }



        //public async Task<string?> LoginAsync(string email, string password)
        //{
        //    var user = await _userRepository.GetQueryable().FirstOrDefaultAsync(u => u.Email == email);
        //    if (user == null)
        //    {
        //        Console.WriteLine("User not found.");
        //        return null;
        //    }

        //    var isMatch = BCrypt.Net.BCrypt.Verify(password, user.Password);
        //    if (!isMatch)
        //    {
        //        Console.WriteLine("Password mismatch.");
        //        return null;
        //    }

        //    var token = _jwtHelper.GenerateToken(user);
        //    return token;
        //}

        public async Task<string?> LoginAsync(string email, string password)
        {
            Console.WriteLine($"🔍 Attempting login for: {email}");

            try
            {
                var user = await _userRepository.GetQueryable().FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    Console.WriteLine("❌ User not found.");
                    return null;
                }

                Console.WriteLine("✅ User found. Checking password...");
                Console.WriteLine($"Stored hash: {user.Password}");

                var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                Console.WriteLine($"🔑 Password match: {isPasswordValid}");

                if (!isPasswordValid)
                {
                    Console.WriteLine("❌ Password mismatch.");
                    return null;
                }

                Console.WriteLine("🔐 Generating token...");
                var token = _jwtHelper.GenerateToken(user);
                Console.WriteLine("✅ Token generated.");

                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 Exception in LoginAsync: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw; // Re-throw so your middleware still handles it
            }
        }




        public async Task LogoutAsync(Guid userId)
        {
            // Example: Set IsActive = 0 or clear session/token
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.IsActive = 0;
                await _userRepository.UpdateAsync(user);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }


        public async Task<User> GetUserById(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }



        //public async Task<User> CreateUserAsync(User user)
        //{
        //    user.Id = Guid.NewGuid();
        //    return await _userRepository.AddAsync(user);

        //}



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
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return false;
            }
            _userRepository.Remove(existingUser);
            return true;
        }

        //public async Task<User?> GetUserByEmail(string email)
        //{
        //    return await _userRepository.GetQueryable().FirstOrDefaultAsync(u => u.Email == email);
        //}



    }
}


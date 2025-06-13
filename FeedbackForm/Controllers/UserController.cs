using FeedbackForm.DTOs;
using FeedbackForm.Models;
using FeedbackForm.Helper; 
using FeedbackForm.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FeedbackForm.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(IUserService _userService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto dto)
        {
            var nameValidation = Utils.NameValidator(dto.Name);
            var emailValidation = Utils.EmailValidator(dto.Email);

            if (!nameValidation.Success || !emailValidation.Success)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                var user = await _userService.RegisterAsync(dto);
                return Ok(new UserDto(user));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            Console.WriteLine("Raw DTO values:");
            Console.WriteLine($"Email: {dto?.Email ?? "null"}");
            Console.WriteLine($"Password: {dto?.Password ?? "null"}");

            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                Console.WriteLine("Model state invalid or missing data.");
                return BadRequest(new { Message = "Invalid input provided." });
            }

            var token = await _userService.LoginAsync(dto.Email, dto.Password);

            if (token == null)
            {
                Console.WriteLine("Login failed — null token.");
                return BadRequest(new { Message = "Invalid email or password." });
            }

            Console.WriteLine("Login success.");
            return Ok(new { Token = token });
        }

        [HttpPost("{id}/logout")]
        public async Task<IActionResult> Logout(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Invalid user ID.");

            await _userService.LogoutAsync(id);
            return Ok("User logged out.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserCreateDto userCreateDto)
        {
            var existingUser = await _userService.GetUserById(id);
            if (existingUser == null)
                return NotFound();
            if (!Utils.NameValidator(userCreateDto.Name).Success || Utils.EmailValidator(userCreateDto.Email).Success)
            {
                return BadRequest("Invalid user data.");
            }
            existingUser.Name = userCreateDto.Name;
            existingUser.Email = userCreateDto.Email;
            var updatedUser = await _userService.UpdateUserAsync(id, existingUser);
            var responseDto = new UserDto(updatedUser);
            return Ok(responseDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();


        }
    }
}

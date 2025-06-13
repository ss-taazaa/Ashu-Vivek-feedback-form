using FeedbackForm.DTOs;
using FeedbackForm.Models;
using FeedbackForm.Helper; 
using FeedbackForm.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FeedbackForm.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UsersController(IUserService _userService) : ControllerBase
    {
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



        [HttpGet("by-email/{email}")]

        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


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
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { Message = "Invalid input provided." });
            }
            var token = await _userService.LoginAsync(dto.Email, dto.Password);
            if (token == null)
            {
                return BadRequest(new { Message = "Invalid email or password." });
            }
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


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserCreateDto userCreateDto)
        {
            var existingUser = await _userService.GetUserById(id);
            if (existingUser == null)
                return NotFound();
            if (!Utils.NameValidator(userCreateDto.Name).Success || !Utils.EmailValidator(userCreateDto.Email).Success)
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
            if (!success.Success)
            {
                return BadRequest(success.Message);
            }
            return Ok();
        }

    }
}

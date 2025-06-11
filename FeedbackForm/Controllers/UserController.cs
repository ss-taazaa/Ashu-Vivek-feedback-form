using FeedbackForm.DTOs;
using FeedbackForm.Models;
using FeedbackForm.Helper; 
using FeedbackForm.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackForm.Controllers
{
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


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            if (!Utils.NameValidator(userCreateDto.Name).Success || !Utils.EmailValidator(userCreateDto.Email).Success)
            {
               return BadRequest("Invalid user data.");
            }
            try
            {
                var user = new User(userCreateDto);
                await _userService.CreateUserAsync(user);
                return Ok("User has been successfully created.");
            }
            catch (Exception)
            {
                return BadRequest("Failed to create user.");
            }
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

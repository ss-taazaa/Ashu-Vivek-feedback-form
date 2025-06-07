using FeedbackForm.DTOs;
using FeedbackForm.Models;
using FeedbackForm.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackForm.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new User(userCreateDto);
            await _userService.CreateUserAsync(user);
            var responseDto = new UserDto(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, responseDto);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserCreateDto userCreateDto)
        {
            var existingUser = await _userService.GetUserById(id);
            if (existingUser == null)
                return NotFound();
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

using FeedbackForm.DTOs;
using FeedbackForm.Models;
using FeedbackForm.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            var user = new User
            {
                Name = userCreateDto.Name,
                Email = userCreateDto.Email,
                CreatedOn = DateTime.UtcNow
            };

            await _userService.CreateUserAsync(user);

            var responseDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedOn = user.CreatedOn,
                FormIds = new List<Guid>() 
            };

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, responseDto);
        }




        // [HttpPut("{id}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserCreateDto userCreateDto)
        {
            var existingUser = await _userService.GetUserById(id);
            if (existingUser == null)
                return NotFound();

            // Update allowed fields
            existingUser.Name = userCreateDto.Name;
            existingUser.Email = userCreateDto.Email;

            var updatedUser = await _userService.UpdateUserAsync(id, existingUser);

            var responseDto = new UserDto
            {
                Id = updatedUser.Id,
                Name = updatedUser.Name,
                Email = updatedUser.Email,
                CreatedOn = updatedUser.CreatedOn,
                FormIds = updatedUser.Forms?.Select(f => f.Id).ToList() ?? new List<Guid>()
            };

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

using Microsoft.AspNetCore.Mvc;
using UserDirectory.Interfaces;
using UserDirectory.Models;

namespace UserDirectory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<User>>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            if (!users.Any())
            {
                return NotFound(new ErrorResponse
                {
                    Status = 404,
                    Message = "No users found."
                });
            }

            return Ok(new ApiResponse<IEnumerable<User>>
            {
                Status = 200,
                Data = users
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> Get(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(new ApiResponse<User>
                {
                    Status = 200,
                    Data = user
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorResponse
                {
                    Status = 404,
                    Message = $"User with ID {id} not found."
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<User>>> Create([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = 400,
                    Message = "Invalid user data.",
                    StackTrace = ModelState.ToString()
                });
            }

            await _userService.AddUserAsync(user);

            return CreatedAtAction(nameof(Get), new { id = user.Id }, new ApiResponse<User>
            {
                Status = 201,
                Data = user
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Status = 400,
                    Message = "Invalid user data."
                });
            }

            var success = await _userService.UpdateUserAsync(id, user);
            if (!success)
            {
                return NotFound(new ErrorResponse
                {
                    Status = 404,
                    Message = $"User with ID {id} not found."
                });
            }

            return Ok(new ApiResponse<User>
            {
                Status = 200,
                Data = user
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                return NotFound(new ErrorResponse
                {
                    Status = 404,
                    Message = $"User with ID {id} not found."
                });
            }

            return Ok(new ApiResponse<string>
            {
                Status = 200,
                Data = $"User {id} deleted"
            });
        }
    }
}

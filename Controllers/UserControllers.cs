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
        public ActionResult<List<User>> GetAll()
        {
            return _userService.GetAllUsers();
        }

        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            _userService.AddUser(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            var success = _userService.UpdateUser(id, user);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _userService.DeleteUser(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}

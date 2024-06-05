using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebRTCChatApp.UserService.Models;
using WebRTCChatApp.UserService.Services.Interfaces;

namespace WebRTCChatApp.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserManagementController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        public UserManagementController(IUserManagementService userServiceRepository)
        {
            _userManagementService = userServiceRepository;
        }

        [Authorize]
        [Route("GetUsers")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(CancellationToken cancellationToken = default)
        {
            try
            {
                var users = await _userManagementService.GetUsers(cancellationToken);
                return users.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving users");
                return NotFound(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id, CancellationToken cancellationToken=default)
        {
            var user = await _userManagementService.GetUserById(id, cancellationToken);

            if (user == null)
            {
                Log.Error("Failed to retrieve user: {Id}", id);
                return NotFound(new { success = false, message = "User not found" });
            }

            return Ok(new { success = true, user });
        }

        [Route("AddUser")]
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(UserDto user, CancellationToken cancellationToken=default)
        {
            var isSuccess = await _userManagementService.AddUser(user, cancellationToken);
            if (!isSuccess)
            {
                Log.Error("Failed to add user.");
                return BadRequest(new { success = false, message = "Failed to add user." });
            }
            return Ok(new { success = true, message = "User added successfully." });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user, CancellationToken cancellationToken=default)
        {
            var isSuccess = await _userManagementService.UpdateUser(id, user, cancellationToken);

            if (!isSuccess)
            {
                Log.Error("Failed to update user: {Id}", id);
                return BadRequest(new { success = false, message = "Failed to update user" });
                }
            return Ok(new { success = true, message = "User updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, CancellationToken cancellationToken= default)
        {
            var isSuccess = await _userManagementService.DeleteUser(id, cancellationToken);

                if (!isSuccess)
            {
                Log.Error("Failed to delete user." );
                return BadRequest(new { success = false, message = "Failed to delete user." });
            }
            return Ok(new { success = true, message = "User deleted successfully." });
        }
    }
}
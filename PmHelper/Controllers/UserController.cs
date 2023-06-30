using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PmHelper.Domain.Services.Users;
using System.Security.Claims;

namespace PmHelper.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> GetUserInfo()
        {
            // Get User Id
            int userId = -1;
            if (!int.TryParse(User.FindFirstValue("sub"), out userId))
            {
                _logger.LogError("UserController::GetUserInfo: User is not authenticated. Can't find user id");
                return BadRequest("Not authenticated");
            }

            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                _logger.LogError($"UserController::GetUserInfo: user with Id={userId} is not found");
                return BadRequest("User is not found");
            }

            _logger.LogInformation($"UserController::GetUserInfo: found user {user.Email} {user.FirstName} {user.LastName}");
            return new JsonResult(user);
        }

        public async Task<IActionResult> DeleteUser()
        {
            // Get User Id
            int userId = -1;
            if (!int.TryParse(User.FindFirstValue("sub"), out userId))
            {
                _logger.LogError("UserController::DeleteUser: User is not authenticated. Can't find user id");
                return BadRequest("Not authenticated");
            }

            try
            {
                _logger.LogInformation($"UserController::DeleteUser: removing user with Id={userId}");
                await _userService.DeleteUserAsync(userId);

                await HttpContext.SignOutAsync();
                return Ok(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserController::DeleteUser: Exception raised. Msg: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

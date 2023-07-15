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
            var userId = (int)HttpContext.Items["userId"]!;

            /*int userId = -1;
            if (!int.TryParse(User.FindFirstValue("sub"), out userId))
            {
                _logger.LogError("UserController::GetUserInfo: User is not authenticated. Can't find user id");
                return BadRequest("Not authenticated");
            }*/
            _logger.LogInformation($"UserController::GetUserInfo: get user information. Id={userId}");
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError($"UserController::GetUserInfo: user with Id={userId} is not found");
                return BadRequest("User is not found");
            }

            _logger.LogInformation($"UserController::GetUserInfo: found user {user.Email} {user.FirstName} {user.LastName}");
            return new JsonResult(user);
        }

        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> DeleteUser(int uid)
        {
            // Get User Id
            var userId = (int)HttpContext.Items["userId"]!;

            var user = await _userService.GetUserByIdAsync(userId);
            if ((user != null) && !user.IsAdmin)
            {
                _logger.LogError("UserController::DeleteUser: user is not admin");
                return BadRequest("The user is not administrator");
            }

            try
            {
                _logger.LogInformation($"UserController::DeleteUser: removing user with Id={uid}");
                await _userService.DeleteUserAsync(uid);

                await HttpContext.SignOutAsync();
                return Ok(uid);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserController::DeleteUser: Exception raised. Msg: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

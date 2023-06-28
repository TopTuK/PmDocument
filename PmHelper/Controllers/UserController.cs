using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PmHelper.Domain.Services.Users;

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
            var userEmail = User.FindFirst("sub")?.Value;
            _logger.LogInformation($"Get user information. Email={userEmail}");

            if (userEmail == null)
            {
                _logger.LogError("User is not authenticated");
                return BadRequest();
            }

            try
            {
                var user = await _userService.GetUserInfoAsync(userEmail);

                _logger.LogInformation($"Get user {user.Email} {user.FirstName} {user.LastName}");
                return new JsonResult(user);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception raised. Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> DeleteUser()
        {
            var userEmail = User.FindFirst("sub")?.Value;
            _logger.LogInformation($"Remove user. Email={userEmail}");

            if (userEmail == null)
            {
                _logger.LogError("User is not authenticated");
                return BadRequest();
            }

            try
            {
                await _userService.DeleteUserAsync(userEmail);

                _logger.LogInformation($"Succesfully delete user with email={userEmail}");
                return Ok(userEmail);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception raised. Message: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}

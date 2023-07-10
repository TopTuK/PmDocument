using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PmHelper.Domain.Services.Users;
using System.Security.Claims;
using System.Text;

namespace PmHelper.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IUserService userService, IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _logger = logger;

            _userService = userService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("AuthController::Logout: start logout");

            // Get User Id
            int userId;
            if (!int.TryParse(User.FindFirstValue("sub"), out userId))
            {
                _logger.LogError("AuthController::Logout: User is not authenticated. Can't find user id");
                return BadRequest("Not authenticated");
            }

            var user = await _userService.GetUserByIdAsync(userId);
            _logger.LogInformation($"AuthController::Logout: {user?.Email}");

            await HttpContext.SignOutAsync();

            return LocalRedirect(new PathString("/"));
        }

        [AllowAnonymous]
        public async Task<IActionResult> SinginCallback()
        {
            _logger.LogInformation("AuthController::SinginCallback: Start authentication callback. Reading the outcome of external auth");

            // Read the outcome of external auth
            var authResult = await HttpContext.AuthenticateAsync(_configuration["AuthTempCookieName"]);

            if (!authResult.Succeeded)
            {
                _logger.LogError("AuthController::SinginCallback: Can't read the outcome of external authentication");
                return LocalRedirect(new PathString("/"));
            }

            _logger.LogInformation("AuthController::SinginCallback: Authentication succeeded. Start reading claims and metadata");

            // Read metadata with scheme
            var metadata = authResult.Properties.Items;
            if ((metadata == null) 
                || (!metadata.ContainsKey("scheme"))
                || (string.IsNullOrEmpty(metadata["scheme"])))
            {
                _logger.LogError("AuthController::SinginCallback: Metadata doesn't contain scheme");
                return LocalRedirect(new PathString("/"));
            }

            var schemeName = metadata["scheme"]!;
            _logger.LogInformation($"AuthController::SinginCallback: Authentication scheme name is \"{schemeName}\"");

            try
            {
                var user = await _userService.AuthenticateAsync(
                    schemeName: schemeName,
                    claims: authResult.Principal.Claims,
                    metadata: metadata!
                );

                _logger.LogInformation($"AuthController::SinginCallback: Authenticated user: {user.Email} {user.FirstName} {user.LastName}");

                var claims = new List<Claim>
                {
                    new("sub", user.Id.ToString()),
                    new("email", user.Email),
                };

                // Run user authentification login (First time seen?)
                //var ci = new ClaimsIdentity(claims, "pwd", "name", "role");
                var ci = new ClaimsIdentity(claims, schemeName);
                var cp = new ClaimsPrincipal(ci);

                await HttpContext.SignInAsync(cp);
                await HttpContext.SignOutAsync("temp");

                _logger.LogInformation("AuthController::SinginCallback: Success SignIn user");

                // return LocalRedirect(new PathString("/auth/secure"));
                return LocalRedirect(new PathString("/profile"));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "AuthController::SinginCallback: Can't authentificate user");
                return LocalRedirect(new PathString("/"));
            }
        }

        [HttpGet]
        public IActionResult SignInGoogle()
        {
            _logger.LogInformation("AuthController::SignInGoogle: Start SignInGoogle authentication");

            var schemeName = _configuration["GoogleAuth:Name"];

            if (schemeName == null)
            {
                _logger.LogError("AuthController::SignInGoogle: scheme name is null");
                return BadRequest("Scheme name is null");
            }

            var props = new AuthenticationProperties
            {
                RedirectUri = new PathString("/auth/SinginCallback"),
                Items =
                {
                    { "scheme", schemeName },
                }
            };

            _logger.LogInformation($"AuthController::SignInGoogle: Start Google challenge with Scheme name {schemeName}");
            return Challenge(props, schemeName);
        }

        [HttpGet]
        public IActionResult SignInOidc()
        {
            _logger.LogInformation("AuthController::SignInOidc: Start SignInOidc authentication");

            var schemeName = _configuration["OidcAuth:Name"];
            
            if (schemeName == null)
            {
                _logger.LogError("AuthController::SignInOidc: scheme name is null");
                return BadRequest("Scheme name is null");
            }

            var props = new AuthenticationProperties
            {
                RedirectUri = new PathString("/auth/SinginCallback"),
                Items =
                {
                    { "scheme", schemeName }
                }
            };

            return Challenge(props, schemeName);
        }
    }
}

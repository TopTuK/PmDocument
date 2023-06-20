﻿using Microsoft.AspNetCore.Authentication;
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

        [AllowAnonymous]
        public IActionResult Index()
        {
            return Content("Anonymous HELLO there");
        }

        [Authorize]
        public IActionResult Secure()
        {
            var claims = User.Claims;

            if ((claims != null) && (claims.Any()))
            {
                var s = new StringBuilder();
                foreach (var c in claims) 
                {
                    s.AppendLine($"{c.Type} -> {c.Value}");
                }

                return Content($"Claims: {s}");
            }
            
            return Content($"No claims IsNull={claims == null}");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return LocalRedirect(new PathString("/auth/Index"));
        }

        [AllowAnonymous]
        public async Task<IActionResult> SinginCallback()
        {
            _logger.LogInformation("Start authentication callback. Reading the outcome of external auth");

            // Read the outcome of external auth
            var authResult = await HttpContext.AuthenticateAsync(_configuration["AuthTempCookieName"]);

            if (!authResult.Succeeded)
            {
                _logger.LogError("Can't read the outcome of external authentication");
                return LocalRedirect(new PathString("/auth/Index"));
            }

            _logger.LogInformation("Authentication succeeded. Start reading claims and metadata");

            // Read metadata with scheme
            var metadata = authResult.Properties.Items;
            if ((metadata == null) 
                || (!metadata.ContainsKey("scheme"))
                || (string.IsNullOrEmpty(metadata["scheme"])))
            {
                _logger.LogError("Metadata doesn't contain scheme");
                return LocalRedirect(new PathString("/auth/Index"));
            }

            try
            {
                // TODO: authenticate user

                var user = await _userService.AuthenticateAsync(
                    schemeName: metadata["scheme"]!,
                    claims: authResult.Principal.Claims,
                    metadata: metadata!
                );

                var claims = new List<Claim>
                {
                    new("sub", "1"),
                    new("name", "Sergey"),
                    new("lastname", "Sidorov"),
                    new("role", "Admin"),
                };

                // Run user authentification login (First time seen?)
                var ci = new ClaimsIdentity(claims, "pwd", "name", "role");
                //var ci = new ClaimsIdentity(claims);
                var cp = new ClaimsPrincipal(ci);

                await HttpContext.SignInAsync(cp);
                await HttpContext.SignOutAsync("temp");

                return LocalRedirect(new PathString("/auth/secure"));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Can't authentificate user");
                return LocalRedirect(new PathString("/auth/Index"));
            }
        }

        [HttpGet]
        public IActionResult SignInGoogle()
        {
            _logger.LogInformation("Start SignInGoogle authentication");

            var schemeName = _configuration["GoogleAuth:Name"];

            if (schemeName == null)
            {
                _logger.LogError("SignInGoogle: scheme name is null");
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

            _logger.LogInformation($"Start Google challenge with Scheme name: {schemeName}");
            return Challenge(props, schemeName);
        }

        [HttpGet]
        public IActionResult SignInOidc()
        {
            _logger.LogInformation("Start SignInOidc authentication");

            var schemeName = _configuration["OidcAuth:Name"];
            
            if (schemeName == null)
            {
                _logger.LogError("SignInOidc: scheme name is null");
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

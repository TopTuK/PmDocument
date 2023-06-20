using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace PmHelper.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {

        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return Content("Hello there");
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

        [HttpGet]
        public async Task<IActionResult> Login(string? user, string pwd)
        {
            if (!string.IsNullOrWhiteSpace(user) && (user == pwd)) 
            {
                var claims = new List<Claim>
                {
                    new("sub", "1"),
                    new("name", "Sergey"),
                    new("role", "Admin"),
                };

                var ci = new ClaimsIdentity(claims, "pwd", "name", "role");
                var cp = new ClaimsPrincipal(ci);

                await HttpContext.SignInAsync(cp);

                return LocalRedirect(new PathString("/auth/secure"));
            }

            return BadRequest();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return LocalRedirect(new PathString("/auth/Index"));
        }

        [AllowAnonymous]
        public async Task<IActionResult> SingIngCallback()
        {
            // Read the outcome of external auth
            var authResult = await HttpContext.AuthenticateAsync("temp");

            if (!authResult.Succeeded)
            {
                return BadRequest("Bad auth");
            }

            var extUser = authResult.Principal;

            var claims = new List<Claim>(extUser.Claims);
            //var sub = extUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var metadata = authResult.Properties.Items;

            if (metadata.ContainsKey("scheme"))
            {

            }

            // Run user authentification login (First time seen?)
            var ci = new ClaimsIdentity(claims, "pwd", "name", "role");
            var cp = new ClaimsPrincipal(ci);

            await HttpContext.SignInAsync(cp);
            await HttpContext.SignOutAsync("temp");

            return LocalRedirect(new PathString("/auth/secure"));
        }

        [HttpGet]
        public IActionResult SignInGoogle()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = new PathString("/auth/SingIngCallback"),
                Items =
                {
                    { "scheme", "google" }
                }
            };

            return Challenge(props, "google");
        }

        [HttpGet]
        public IActionResult SignInOidc()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = new PathString("/auth/SingIngCallback"),
                Items =
                {
                    { "scheme", "oidc" }
                }
            };

            return Challenge(props, "oidc");
        }
    }
}

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

        [AllowAnonymous]
        public IActionResult Index()
        {
            return Content("Hello there");
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
            return await Task.FromResult(BadRequest());
        }

        [HttpGet]
        public IActionResult SignInGoogle()
        {
            var props = new AuthenticationProperties
            {

            };

            return Challenge(props, "google");
        }
    }
}

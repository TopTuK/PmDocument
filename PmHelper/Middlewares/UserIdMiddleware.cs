using System.Security.Claims;

namespace PmHelper.Middlewares
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;

        public UserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.User != null)
            {
                if ((httpContext.User.Identity != null) && (httpContext.User.Identity.IsAuthenticated))
                {
                    if (int.TryParse(httpContext.User.FindFirstValue("sub"), out var userId))
                    {
                        httpContext.Items.Add("userId", userId);
                    }
                }
            }

            await _next(httpContext);
        }
    }
}

using PmHelper.Domain.Models.Users;
using PmHelper.Domain.Repository;
using System.Security.Claims;

namespace PmHelper.Domain.Services.Users
{
    internal class UserService : IUserService
    {
        private class User : IUser
        {
            public string Id => throw new NotImplementedException();
            public string FirstName => throw new NotImplementedException();
            public string LastName => throw new NotImplementedException();
            public string Email => throw new NotImplementedException();
        }

        private readonly ILogger<IUserService> _logger;
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext, ILogger<IUserService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IUser> AuthenticateAsync(string schemeName,
            IEnumerable<Claim> claims, IDictionary<string, string> metadata)
        {
            _logger.LogInformation($"Start authenticating user with scheme \"{schemeName}\"");

            return await Task.FromResult(new User());

            //var sub = extUser.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PmHelper.Domain.Models;
using PmHelper.Domain.Models.Users;
using PmHelper.Domain.Repository;
using PmHelper.Domain.Repository.Entities;
using System.Security.Claims;

namespace PmHelper.Domain.Services.Users
{
    internal class UserService : IUserService
    {
        private record User : IUser
        {
            public string FirstName { get; init; } = string.Empty;
            public string LastName { get; init; } = string.Empty;
            public string Email { get; init; } = string.Empty;

            public User(DbUser dbUser)
            {
                Email = dbUser.Email!;
                FirstName = dbUser.FirstName ?? "Anonymous";
                LastName = dbUser.LastName ?? string.Empty;
            }
        }

        private record UserInfo
        {
            public string? Sub { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
        }

        private readonly ILogger<IUserService> _logger;
        private readonly AppDbContext _dbContext;

        private readonly string _googleSchemeName;
        private readonly string _oidcSchemeName;

        public UserService(AppDbContext dbContext, IConfiguration configuration,
            ILogger<IUserService> logger)
        {
            _dbContext = dbContext;

            _googleSchemeName = configuration["GoogleAuth:Name"] ?? "google";
            _oidcSchemeName = configuration["OidcAuth:Name"] ?? "oidc";

            _logger = logger;
        }

        // https://www.milanjovanovic.tech/blog/working-with-transactions-in-ef-core
        public async Task<IUser> GetOrCreateUserAsync(string email, string? firstName, string? lastName)
        {
            try
            {
                var dbUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (dbUser == null)
                {
                    dbUser = new DbUser
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email
                    };

                    await _dbContext.Users.AddAsync(dbUser);
                    await _dbContext.SaveChangesAsync();

                    return new User(dbUser);
                }
                else
                {
                    return new User(dbUser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Can't get or create user. Msg: {}", ex.Message);
                throw;
            }
        }

        private static UserInfo GetGoogleUserInfo(IEnumerable<Claim> claims) => new()
        {
            Sub = claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                .Value,
            FirstName = claims
                .FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?
                .Value,
            LastName = claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Surname)?
                .Value,
            Email = claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?
                .Value,
        };

        private static UserInfo GetOidcUserInfo(IEnumerable<Claim> claims)
        {
            return new();
        }

        public async Task<IUser> AuthenticateAsync(string schemeName,
            IEnumerable<Claim> claims, IDictionary<string, string> metadata)
        {
            _logger.LogInformation($"Start authenticating user with scheme \"{schemeName}\"");

            if (schemeName == _googleSchemeName)
            {
                var userInfo = GetGoogleUserInfo(claims);

                if (userInfo.Email == null)
                {
                    _logger.LogError("GoogleAuthScheme: user email claim is null");
                    throw new AuthenticationException("GoogleAuthScheme: user email claim is null");
                }

                var user = await GetOrCreateUserAsync(userInfo.Email, userInfo.FirstName, userInfo.LastName);

                return user;
            }
            else if (schemeName == _oidcSchemeName)
            {
                var userInfo = GetOidcUserInfo(claims);

                if (userInfo.Email == null)
                {
                    _logger.LogError("OidcAuthScheme: user email claim is null");
                    throw new AuthenticationException("OidcScheme: user email claim is null");
                }

                var user = await GetOrCreateUserAsync(userInfo.Email, userInfo.FirstName, userInfo.LastName);

                return user;
            }
            else
            {
                _logger.LogCritical($"Unknown scheme name \"{schemeName}\"");
                throw new AuthenticationException($"Unknown scheme name \"{schemeName}\"");
            }
        }
    }
}

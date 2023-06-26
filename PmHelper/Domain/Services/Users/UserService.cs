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
                _logger.LogInformation($"Get or Create user with email: {email}");
                var dbUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == email);

                _logger.LogInformation($"User with {email} found result: {dbUser == null}");
                if (dbUser == null)
                {
                    dbUser = new DbUser
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email
                    };

                    _logger.LogInformation($"Creating new user: {email} {firstName} {lastName}");

                    await _dbContext.Users.AddAsync(dbUser);
                    await _dbContext.SaveChangesAsync();

                    return new User(dbUser);
                }
                else
                {
                    _logger.LogInformation($"User exists: {dbUser.Email} {dbUser.FirstName} {dbUser.LastName}");
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
            var sub = claims
                .FirstOrDefault(c => c.Type == "sub")?
                .Value;
            var name = claims
                .FirstOrDefault(c => c.Type == "name")?
                .Value;
            var email = claims
                .FirstOrDefault(c => c.Type == "email")?
                .Value;

            if (name != null)
            {
                var splitName = name.Split(' ');
                var firstName = splitName[0];
                var lastName = string.Empty;

                if (splitName.Length > 1)
                {
                    lastName = splitName[1].Trim();
                }

                return new()
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Sub = sub,
                };
            }

            return new()
            {
                Email = email,
                FirstName = sub ?? "Anonymous",
                LastName = string.Empty,
                Sub = sub,
            };
        }

        private UserInfo GetUserInfo(string schemeName, IEnumerable<Claim> claims)
        {
            if (schemeName == _googleSchemeName)
            {
                return GetGoogleUserInfo(claims);
            }
            else if (schemeName == _oidcSchemeName)
            {
                return GetOidcUserInfo(claims);
            }
            else
            {
                _logger.LogCritical($"Unknown scheme name \"{schemeName}\"");
                throw new AuthenticationException($"Unknown scheme name \"{schemeName}\"");
            }
        }

        public async Task<IUser> AuthenticateAsync(string schemeName,
            IEnumerable<Claim> claims, IDictionary<string, string> metadata)
        {
            _logger.LogInformation($"Start authenticating user with scheme \"{schemeName}\"");

            var userInfo = GetUserInfo(schemeName, claims);
            _logger.LogInformation($"UserInfo: {userInfo.Email} {userInfo.FirstName} {userInfo.LastName}");

            if (userInfo.Email == null)
            {
                _logger.LogError("GoogleAuthScheme: user email claim is null");
                throw new AuthenticationException("GoogleAuthScheme: user email claim is null");
            }

            var user = await GetOrCreateUserAsync(userInfo.Email, userInfo.FirstName, userInfo.LastName);

            return user;
        }

        public async Task<IUser> GetUserInfoAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}

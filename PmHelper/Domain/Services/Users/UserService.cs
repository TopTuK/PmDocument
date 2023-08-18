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
            public int Id { get; init; }
            public string FirstName { get; init; } = string.Empty;
            public string LastName { get; init; } = string.Empty;
            public string Email { get; init; } = string.Empty;

            public bool IsAdmin { get; init; } = false;

            public User(DbUser dbUser)
            {
                Id = dbUser.Id;
                Email = dbUser.Email!;
                FirstName = dbUser.FirstName ?? "Anonymous";
                LastName = dbUser.LastName ?? string.Empty;

                IsAdmin = dbUser.IsAdmin;
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

        private readonly List<string> _adminEmails = new();

        public UserService(AppDbContext dbContext, IConfiguration configuration,
            ILogger<IUserService> logger)
        {
            _dbContext = dbContext;

            _googleSchemeName = configuration["GoogleAuth:Name"] ?? "google";
            _oidcSchemeName = configuration["OidcAuth:Name"] ?? "oidc";

            var adminEmails = configuration["AdminEmails"]?.Split(',');
            if (adminEmails != null)
            {
                _adminEmails.AddRange(adminEmails);
            }

            _logger = logger;
        }

        // https://www.milanjovanovic.tech/blog/working-with-transactions-in-ef-core
        public async Task<IUser> GetOrCreateUserAsync(string email, string? firstName, string? lastName)
        {
            try
            {
                _logger.LogInformation($"UserService::GetOrCreateUserAsync: Get or Create user with email: {email}");
                
                var dbUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == email);
                _logger.LogInformation($"UserService::GetOrCreateUserAsync: User with {email} found result: {dbUser == null}");

                if (dbUser == null) // Create user
                {
                    dbUser = new DbUser
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                    };

                    if (IsUserAdmin(email))
                    {
                        _logger.LogWarning("UserService::GetOrCreateUserAsync: creating user with admin rights. Email={}", email);
                        dbUser.IsAdmin = true;
                    }

                    _logger.LogInformation($"UserService::GetOrCreateUserAsync: Creating new user: {email} {firstName} {lastName}");

                    await _dbContext.Users.AddAsync(dbUser);
                    await _dbContext.SaveChangesAsync();

                    return new User(dbUser);
                }
                else
                {
                    _logger.LogInformation($"UserService::GetOrCreateUserAsync: User exists: {dbUser.Id} {dbUser.Email} {dbUser.FirstName} {dbUser.LastName}");
                    return new User(dbUser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("UserService::GetOrCreateUserAsync: Can't get or create user. Msg: {}", ex.Message);
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
                _logger.LogCritical($"UserService::GetUserInfo: Unknown scheme name \"{schemeName}\"");
                throw new AuthenticationException($"Unknown scheme name \"{schemeName}\"");
            }
        }

        public async Task<IUser> AuthenticateAsync(string schemeName,
            IEnumerable<Claim> claims, IDictionary<string, string> metadata)
        {
            _logger.LogInformation($"UserService::AuthenticateAsync: Start authenticating user with scheme \"{schemeName}\"");

            var userInfo = GetUserInfo(schemeName, claims);
            _logger.LogInformation($"UserService::AuthenticateAsync: UserInfo: {userInfo.Email} {userInfo.FirstName} {userInfo.LastName}");

            if (userInfo.Email == null)
            {
                _logger.LogError("UserService::AuthenticateAsync: GoogleAuthScheme: user email claim is null");
                throw new AuthenticationException("GoogleAuthScheme: user email claim is null");
            }

            var user = await GetOrCreateUserAsync(userInfo.Email, userInfo.FirstName, userInfo.LastName);

            return user;
        }

        public async Task<IUser?> GetUserByIdAsync(int userId)
        {
            _logger.LogInformation($"UserService::GetUserById: Get user by Id={userId}");

            try
            {
                var dbUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (dbUser == null)
                {
                    _logger.LogWarning($"UserService::GetUserById: User with Id={userId} is not found.");
                    return null;
                }
                else
                {
                    _logger.LogInformation($"UserService::GetUserById: Found user {dbUser.Email} {dbUser.FirstName} {dbUser.LastName}");
                    return new User(dbUser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"UserService::GetUserById: Exception raised. Msg: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            _logger.LogInformation($"UserService::DeleteUserAsync: Delete user with Id={userId}");

            try
            {
                var dbUser = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (dbUser == null)
                {
                    _logger.LogCritical($"UserService::DeleteUserAsync: User with Id={userId} is not found!");
                    throw new UserException();
                }

                _dbContext.Remove(dbUser);
                await _dbContext.SaveChangesAsync();
                _logger.LogWarning($"UserService::DeleteUserAsync: Removed user with Id={userId} from database");
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"UserService::DeleteUserAsync:: Exception raised. MSg: {ex.Message}");
                throw;
            }
        }

        public bool IsUserAdmin(string email) => _adminEmails.Contains(email);

        public async Task<IEnumerable<IUser>> GetAllUsersAsync()
        {
            _logger.LogInformation($"UserService::GetAllUsers: function call");

            var users = await _dbContext.Users
                .Select(dbUser => new User(dbUser))
                .ToListAsync();

            _logger.LogInformation("UserService::GetAllUsers: return {} users", users.Count);
            return users;
        }
    }
}

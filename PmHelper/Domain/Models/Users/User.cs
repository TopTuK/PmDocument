using PmHelper.Domain.Repository.Entities;

namespace PmHelper.Domain.Models.Users
{
    internal record User : IUser
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
}

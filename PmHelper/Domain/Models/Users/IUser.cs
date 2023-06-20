using System.ComponentModel.DataAnnotations;

namespace PmHelper.Domain.Models.Users
{
    public interface IUser
    {
        string Id { get; }

        string FirstName { get; }
        
        string LastName { get; }
        
        [EmailAddress]
        string Email { get; }
    }
}

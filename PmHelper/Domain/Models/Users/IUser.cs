using System.ComponentModel.DataAnnotations;

namespace PmHelper.Domain.Models.Users
{
    public interface IUser
    {
        string FirstName { get; }
        
        string LastName { get; }
        
        [EmailAddress]
        string Email { get; }
    }
}

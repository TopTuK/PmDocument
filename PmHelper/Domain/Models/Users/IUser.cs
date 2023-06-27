using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

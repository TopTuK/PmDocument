using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PmHelper.Domain.Models.Users
{
    public interface IUser
    {
        int Id { get; }

        string FirstName { get; }
        
        string LastName { get; }
        
        [EmailAddress]
        string Email { get; }

        bool IsAdmin { get => false; }
    }
}

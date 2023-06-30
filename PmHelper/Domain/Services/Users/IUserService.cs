using PmHelper.Domain.Models.Users;
using System.Security.Claims;

namespace PmHelper.Domain.Services.Users
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="schemeName"></param>
        /// <param name="claims"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Task<IUser> AuthenticateAsync(string schemeName,
            IEnumerable<Claim> claims, IDictionary<string, string> metadata);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IUser?> GetUserById(int userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteUserAsync(int userId);
    }
}

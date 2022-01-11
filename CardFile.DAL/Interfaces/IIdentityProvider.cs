using CardFile.DAL.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CardFile.DAL.Interfaces
{
    public interface IIdentityProvider
    {
        Task<bool> CreateUser(UserAuthInfo user);
        Task<bool> CreateRole(string role);
        Task<bool> GiveRoleToUser(string role, string username);
        Task<bool> RemoveUserFromRole(string username, string role);
        Task<ClaimsIdentity> GetUserClaim(UserAuthInfo user);
    }
}

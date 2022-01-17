using CardFile.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CardFile.DAL.Interfaces
{
    public interface IIdentityProvider
    {
        IEnumerable<IdentityUser> GetUsers();
        List<string> GetRoles();
        Task<IList<string>> GetUserRoles(string username);
        Task<IdentityResult> CreateUser(UserAuthInfo user);
        Task<bool> CreateRole(string role);
        Task<bool> GiveRoleToUser(string role, string username);
        Task<bool> RemoveUserFromRole(string username, string role);
        Task<bool> RemoveUserFromAllRoles(string username);
        Task<ClaimsIdentity> GetUserClaim(UserAuthInfo user);
    }
}

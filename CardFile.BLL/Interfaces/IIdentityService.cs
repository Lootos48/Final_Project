using CardFile.BLL.DTO;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    public interface IIdentityService
    {
        IEnumerable<IdentityUser> GetUsers();
        IEnumerable<IdentityRole> GetRoles(string username);
        Task<bool> CreateUser(UserAuthInfoDTO user);
        Task<bool> CreateRole(string role);
        Task<bool> GiveRoleToUser(string role, string username);
        Task<bool> RemoveUserFromRole(string username, string role);
        Task<ClaimsIdentity> GetUserClaims(UserAuthInfoDTO user);
    }
}

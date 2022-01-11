using CardFile.BLL.DTO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> CreateUser(UserAuthInfoDTO user);
        Task<bool> CreateRole(string role);
        Task<bool> GiveRoleToUser(string role, string username);
        Task<bool> RemoveUserFromRole(string username, string role);
        Task<ClaimsIdentity> GetUserClaims(UserAuthInfoDTO user);
    }
}

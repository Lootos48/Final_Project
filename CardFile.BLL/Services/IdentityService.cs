using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Infrastructure;
using CardFile.BLL.Interfaces;
using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IIdentityProvider identityProvider;
        private readonly IMapper mapper;
        public IdentityService(IIdentityProvider identityProvider)
        {
            this.identityProvider = identityProvider;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserAuthInfoDTO, UserAuthInfo>();
            });

            mapper = config.CreateMapper();
        }
        public async Task<bool> CreateUser(UserAuthInfoDTO user)
        {
            bool result = await identityProvider.CreateUser(mapper.Map<UserAuthInfo>(user));
            if (result)
            {
                result = await GiveRoleToUser("RegisteredUser", user.Username);
            }
            else
            {
                throw new ValidationException("User with that username is already exist", "Username");
            }
            return result;
        }
        public async Task<bool> CreateRole(string role)
        {
            return await identityProvider.CreateRole(role);
        }

        public async Task<bool> GiveRoleToUser(string role, string username)
        {
            return await identityProvider.GiveRoleToUser(role, username);
        }
        public async Task<bool> RemoveUserFromRole(string username, string role)
        {
            return await identityProvider.RemoveUserFromRole(username, role);
        }

        public async Task<ClaimsIdentity> GetUserClaims(UserAuthInfoDTO user)
        {
            ClaimsIdentity claims = await identityProvider.GetUserClaim(mapper.Map<UserAuthInfo>(user));
            if (claims == null)
            {
                throw new ValidationException("User with that login info isn`t exist", "Username");
            }
            return claims;
        }

    }
}

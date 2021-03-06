using AutoMapper;
using CardFile.BLL.DTO;
using CardFile.BLL.Infrastructure;
using CardFile.BLL.Interfaces;
using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Services
{
    /// <summary>
    /// Класс реализации интерфейса <see cref="IIdentityService"/>
    /// <inheritdoc cref="IIdentityService"/>
    /// </summary>
    public class IdentityService : IIdentityService
    {
        /// <summary>
        /// Поле для вызова CRUD-операций интерфейса <see cref="IIdentityProvider"/>
        /// </summary>
        private readonly IIdentityProvider identityProvider;

        /// <summary>
        /// Поле для инициализации маппера
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Конструктор, который принимает интерфейс <see cref="IIdentityService"/>,
        /// Задаёт конфигурацию авто-маппера и инициализирует поля класса
        /// </summary>
        /// <param name="identityProvider">Интерфейс <see cref="IIdentityService"/> устанавливается с помощью IoC-контейнера</param>
        public IdentityService(IIdentityProvider identityProvider)
        {
            this.identityProvider = identityProvider;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserAuthInfoDTO, UserAuthInfo>();
                cfg.CreateMap<IdentityUser, UserInfoDTO>()
                    .ForMember(x => x.Username, y => y.MapFrom(c => c.UserName));
            });

            mapper = config.CreateMapper();
        }

        public IEnumerable<IdentityUser> GetUsers()
        {
            return identityProvider.GetUsers();
        }
        public List<string> GetRoles()
        {
            return identityProvider.GetRoles();
        }

        public async Task<IList<string>> GetUserRoles(string username)
        {
            return await identityProvider.GetUserRoles(username);
        }

        public async Task<bool> CreateUser(UserAuthInfoDTO user)
        {
            IdentityResult isCreated = await identityProvider.CreateUser(mapper.Map<UserAuthInfo>(user));
            if (isCreated.Succeeded)
            {
                bool isRegistered = await GiveRoleToUser("RegisteredUser", user.Username);
                return isRegistered;
            }
            else
            {
                throw new ValidationException(string.Join(".", isCreated.Errors), "");
            }
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
        public async Task<bool> RemoveUserFromAllRoles(string username)
        {
            return await identityProvider.RemoveUserFromAllRoles(username);
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

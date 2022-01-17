﻿using CardFile.DAL.EF;
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

namespace CardFile.DAL.Repositories
{
    public class IdentityProvider : IIdentityProvider
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;

        public IdentityProvider()
        {
            var roleStore = new RoleStore<IdentityRole>();
            _roleManager = new RoleManager<IdentityRole>(roleStore);

            var userStore = new UserStore<IdentityUser>();
            _userManager = new UserManager<IdentityUser>(userStore);

            _userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 3,
                RequireUppercase = true
            };
        }

        public IEnumerable<IdentityUser> GetUsers()
        {
            return _userManager.Users;
        }

        public List<string> GetRoles()
        {
            List<string> roles = new List<string>();
            foreach (var role in _roleManager.Roles)
            {
                roles.Add(role.Name);
            }
            return roles;
        }


        public async Task<IList<string>> GetUserRoles(string username)
        {
            string userId = _userManager.FindByName(username).Id;
            return await _userManager.GetRolesAsync(userId);
        }

        public async Task<IdentityResult> CreateUser(UserAuthInfo user)
        {
            var identityUser = new IdentityUser(user.Username);
            var result = await _userManager.CreateAsync(identityUser, user.Password);

            return result;
        }

        public async Task<bool> CreateRole(string role)
        {
            var identityRole = new IdentityRole(role);
            var result = await _roleManager.CreateAsync(identityRole);
            return result.Succeeded;

        }

        public async Task<bool> GiveRoleToUser(string role, string username)
        {
            var userId = _userManager.Users.First<IdentityUser>(u => u.UserName == username).Id;
            var result = await _userManager.AddToRoleAsync(userId, role);
            return result.Succeeded;
        }
        public async Task<bool> RemoveUserFromRole(string username, string role)
        {
            var userId = _userManager.Users.First<IdentityUser>(u => u.UserName == username).Id;
            var result = await _userManager.RemoveFromRoleAsync(userId, role);
            return result.Succeeded;
        }

        public async Task<ClaimsIdentity> GetUserClaim(UserAuthInfo user)
        {
            var identityUser = await _userManager.FindAsync(user.Username, user.Password);
            if (identityUser == null)
            {
                return null;
            }
            var userIdentity = _userManager.CreateIdentity(identityUser, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public async Task<bool> RemoveUserFromAllRoles(string username)
        {
            var userId = _userManager.Users.First(u => u.UserName == username).Id;
            string[] userRoles = (await GetUserRoles(username)).ToArray();
            var result = await _userManager.RemoveFromRolesAsync(userId, userRoles);
            return result.Succeeded;
        }

    }
}

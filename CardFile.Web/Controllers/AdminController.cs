using CardFile.BLL.Interfaces;
using CardFile.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CardFile.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IIdentityService identityService;
        private readonly IAuthorsService authorsService;
        public AdminController(IIdentityService identityService, IAuthorsService authorsServ)
        {
            authorsService = authorsServ;
            this.identityService = identityService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var author = (await authorsService.GetAll()).First(y => y.Username == "User");
            IEnumerable<IdentityRole> auth = identityService.GetRoles(author.Username);
            List<string> roles = new List<string>();
            foreach (var item in auth)
            {
                roles.Add(item.Name);
            }
            IEnumerable<UserInfoViewModel> userInfos = new List<UserInfoViewModel>()
            {
                new UserInfoViewModel()
                {
                    Username = author.Username,
                    FirstName = author.FirstName,
                    LastName = author.SecondName,
                    Roles = roles.ToArray() 
                }
            };

            return View(userInfos);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateRole(string role)
        {
            var result = await identityService.CreateRole(role);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("CreateRoleFailView");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveUserFromRole(string username, string role)
        {
            var result = await identityService.RemoveUserFromRole(username, role);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("RemoveRoleToUserFailView");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GiveRoleToUser(string role, string username)
        {
            var result = await identityService.GiveRoleToUser(role, username);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("GiveRoleToUserFailView");
        }
    }
}
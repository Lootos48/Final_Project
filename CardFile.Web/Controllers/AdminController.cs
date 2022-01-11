using CardFile.BLL.Interfaces;
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
        public AdminController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View();
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
using CardFile.BLL.Infrastructure;
using CardFile.BLL.Interfaces;
using CardFile.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CardFile.Web.Controllers
{
    /// <summary>
    /// Контроллер для функций администрирования
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// Поле для взаимодействия с пользователями
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Поле для взаимодействия с профилями пользователей
        /// </summary>
        private readonly IAuthorsService _authorsService;
        public AdminController(IIdentityService identityService, IAuthorsService authorsServ)
        {
            _authorsService = authorsServ;
            this._identityService = identityService;
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            var authors = await _authorsService.GetAll();
            ICollection<UserInfoViewModel> userInfos = new List<UserInfoViewModel>();
            foreach (var author in authors)
            {
                List<string> userRole = (List<string>)await _identityService.GetUserRoles(author.Username);

                userInfos.Add(new UserInfoViewModel()
                {
                    Username = author.Username,
                    FirstName = author.FirstName,
                    LastName = author.SecondName,
                    Roles = userRole.ToArray()
                });
            }
            ViewBag.AllRoles = _identityService.GetRoles();

            return View(userInfos);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateRole(string role)
        {
            var result = await _identityService.CreateRole(role);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveUserFromRole(string username, string role)
        {
            var result = await _identityService.RemoveUserFromRole(username, role);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return new ViewResult() { ViewName = "~/Views/Shared/Error.cshtml" };
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GiveRoleToUser(string role, string username)
        {
            var result = await _identityService.GiveRoleToUser(role, username);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return new ViewResult() { ViewName = "~/Views/Shared/Error.cshtml" };
        }

        public async Task<ActionResult> BanUser(string username)
        {
            var result = await _identityService.RemoveUserFromAllRoles(username);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return new ViewResult() { ViewName = "~/Views/Shared/Error.cshtml" };
        }
    }
}
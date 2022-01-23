using NUnit.Framework;
using CardFile.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using CardFile.BLL.Interfaces;
using System.Web.Mvc;
using System.Net;
using CardFile.BLL.Infrastructure;

namespace CardFile.Web.Controllers.Tests
{
    [TestFixture()]
    public class AdminControllerTests
    {
        readonly Mock<IIdentityService> identityServiceMock = new Mock<IIdentityService>();
        readonly Mock<IAuthorsService> authorServiceMock = new Mock<IAuthorsService>();

        AdminController controller;

        [SetUp]
        public void Setup()
        {
            controller = new AdminController(identityServiceMock.Object, authorServiceMock.Object);
        }



        /*[Test()]
        public async Task Delete_ParameterIsNull_ReturnedBadRequestStatusCode()
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            HttpStatusCodeResult result = await controller.Delete(new int?()) as HttpStatusCodeResult;

            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [Test()]
        public async Task Delete_AuthorIsNotFound_ReturnedNotFoundStatusCode()
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            authorServiceMock.Setup(obj => obj.DeleteAuthor(It.IsAny<int>())).Throws(new ObjectNotFoundException(""));

            HttpStatusCodeResult result = await controller.Delete(2) as HttpStatusCodeResult;

            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [Test()]
        public async Task Delete_AuthorIsDeleted_ReturnedRedirectToAction()
        {
            var expected = "Index";
            authorServiceMock.Setup(obj => obj.DeleteAuthor(It.IsAny<int>())).ReturnsAsync(true);

            RedirectToRouteResult result = await controller.Delete(2) as RedirectToRouteResult;

            Assert.AreEqual(expected, result.RouteValues["action"]);
        }*/

        [Test()]
        public async Task CreateRole_RoleCreated()
        {
            string expected = "Index";
            identityServiceMock.Setup(obj => obj.CreateRole(It.IsAny<string>())).ReturnsAsync(true);

            RedirectToRouteResult result = await controller.CreateRole("") as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [Test()]
        public async Task CreateRole_RoleNotCreated()
        {
            string expected = "~/Views/Shared/Error.cshtml";
            identityServiceMock.Setup(obj => obj.CreateRole(It.IsAny<string>())).ReturnsAsync(false);

            ViewResult result = await controller.CreateRole("") as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task RemoveUserFromRole_UserRemovedFromRole()
        {
            string expected = "Index";
            identityServiceMock.Setup(obj => obj.RemoveUserFromRole(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            RedirectToRouteResult result = await controller.RemoveUserFromRole("", "") as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [Test()]
        public async Task RemoveUserFromRole_UserNotRemovedFromRole()
        {
            string expected = "~/Views/Shared/Error.cshtml";
            identityServiceMock.Setup(obj => obj.RemoveUserFromRole(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            ViewResult result = await controller.RemoveUserFromRole("", "") as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task GiveRoleToUser_RoleWasGived()
        {
            string expected = "Index";
            identityServiceMock.Setup(obj => obj.GiveRoleToUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            RedirectToRouteResult result = await controller.GiveRoleToUser("", "") as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [Test()]
        public async Task GiveRoleToUser_RoleWasNotGived()
        {
            string expected = "~/Views/Shared/Error.cshtml";
            identityServiceMock.Setup(obj => obj.RemoveUserFromRole(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            ViewResult result = await controller.RemoveUserFromRole("", "") as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task BanUser_UserWasNotBanned()
        {

            string expected = "~/Views/Shared/Error.cshtml";
            identityServiceMock.Setup(obj => obj.RemoveUserFromAllRoles(It.IsAny<string>())).ReturnsAsync(false);

            ViewResult result = await controller.BanUser("") as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task BanUser_UserWasBanned()
        {
            string expected = "Index";
            identityServiceMock.Setup(obj => obj.RemoveUserFromAllRoles(It.IsAny<string>())).ReturnsAsync(true);

            RedirectToRouteResult result = await controller.BanUser("") as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }
    }
}
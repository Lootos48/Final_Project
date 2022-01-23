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
using CardFile.BLL.DTO;
using System.Web;
using System.Security.Claims;
using CardFile.Web.Models;

namespace CardFile.Web.Controllers.Tests
{
    [TestFixture()]
    public class AuthorControllerTests
    {
        readonly Mock<IIdentityService> identityServiceMock = new Mock<IIdentityService>();
        readonly Mock<IAuthorsService> authorServiceMock = new Mock<IAuthorsService>();

        AuthorController controller;

        [SetUp]
        public void Setup()
        {
            controller = new AuthorController(authorServiceMock.Object, identityServiceMock.Object);

        }

        [Test()]
        public async Task Details_ParameterIsNull_ReturnsBadRequestStatusCode()
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            HttpStatusCodeResult result = await controller.Details(null) as HttpStatusCodeResult;

            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [Test()]
        public async Task Details_AuthorNotExist_ReturnsNotFoundStatusCode()
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            authorServiceMock.Setup(obj => obj.GetAuthor(It.IsAny<int>())).Throws(new ObjectNotFoundException(""));

            HttpStatusCodeResult result = await controller.Details(456) as HttpStatusCodeResult;

            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [Test()]
        public async Task Details_AuthorIsExist_ReturnsView()
        {
            string expected = "Details";
            authorServiceMock.Setup(obj => obj.GetAuthor(It.IsAny<int>())).ReturnsAsync(new AuthorDTO());

            ViewResult result = await controller.Details(10) as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [TestCase("")]
        [TestCase(null)]
        public async Task AuthorProfile_ParameterIsNull_ReturnsBadRequestStatusCode(string username)
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            HttpStatusCodeResult result = await controller.AuthorProfile(username) as HttpStatusCodeResult;

            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [Test()]
        public async Task AuthorProfile_AuthorIsNotExist_ReturnsNotFoundStatusCode()
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            authorServiceMock.Setup(obj => obj.GetAuthor(It.IsAny<Func<AuthorDTO, bool>>()));


            HttpStatusCodeResult result = await controller.AuthorProfile("test") as HttpStatusCodeResult;

            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [Test()]
        public async Task AuthorProfile_AuthorIsExist_ReturnsView()
        {
            string expected = "Details";
            authorServiceMock.Setup(obj => obj.GetAuthor(It.IsAny<Func<AuthorDTO, bool>>())).ReturnsAsync(new AuthorDTO());

            ViewResult result = await controller.AuthorProfile("test") as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task Login_ModelNotValid_ReturnsView()
        {
            string expected = "Login";
            controller.ModelState.AddModelError("", "");

            ViewResult result = await controller.Login(new LoginViewModel()) as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task Login_UserNotExist_ReturnsView()
        {
            string expected = "Login";
            identityServiceMock.Setup(obj => obj.GetUserClaims(It.IsAny<UserAuthInfoDTO>())).Throws(new ValidationException("", ""));

            ViewResult result = await controller.Login(new LoginViewModel()) as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task Registration_ModelIsNotValid_ReturnsView()
        {
            string expected = "Registration";
            controller.ModelState.AddModelError("", "");

            ViewResult result = await controller.Registration(new RegistrationViewModel()) as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task Registration_UserAlreadyExist_ReturnsView()
        {
            string expected = "Registration";
            identityServiceMock.Setup(obj => obj.CreateUser(It.IsAny<UserAuthInfoDTO>())).Throws(new ValidationException("", ""));

            ViewResult result = await controller.Registration(new RegistrationViewModel()) as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task Edit_ParameterIsNull_ReturnedBadRequestStatusCode()
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            HttpStatusCodeResult result = await controller.Edit(new int?()) as HttpStatusCodeResult;

            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }
    }
}
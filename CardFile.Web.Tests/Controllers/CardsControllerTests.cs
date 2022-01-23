using NUnit.Framework;
using CardFile.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardFile.BLL.Interfaces;
using Moq;
using CardFile.BLL.DTO;
using System.Web.Mvc;
using System.Net;
using Microsoft.Owin.Security;
using CardFile.BLL.Infrastructure;
using CardFile.Web.Models;
using Microsoft.Owin;

namespace CardFile.Web.Controllers.Tests
{
    [TestFixture()]
    public class CardsControllerTests
    {
        Mock<ICardsService> cardServiceMock = new Mock<ICardsService>();
        Mock<ILikeService> likeServiceMock = new Mock<ILikeService>();
        Mock<IAuthorsService> authorServiceMock = new Mock<IAuthorsService>();
        Mock<IAuthenticationManager> authManagerMock = new Mock<IAuthenticationManager>();

        CardsController controller;

        [SetUp]
        public void Setup()
        {
            controller = new CardsController(cardServiceMock.Object, authorServiceMock.Object, likeServiceMock.Object);

        }

        [Test()]
        public async Task Like_OperationSuccess_RedirectedToAction()
        {
            string expected = "Details";
            authorServiceMock.Setup(obj => obj.GetAuthor(It.IsAny<Func<AuthorDTO, bool>>())).ReturnsAsync(new AuthorDTO());
            likeServiceMock.Setup(obj => obj.LikeCard(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

            RedirectToRouteResult result = await controller.Like(10) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [Test()]
        public async Task Like_AuthorNotExist_ThrowsStatusCodeResult()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            authorServiceMock.Setup(obj => obj.GetAuthor(It.IsAny<Func<AuthorDTO, bool>>()));

            HttpStatusCodeResult result = await controller.Like(10) as HttpStatusCodeResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [Test()]
        public async Task Like_UnhandledError_ReturnsErrorView()
        {
            string expected = "~/Views/Shared/Error.cshtml";
            authorServiceMock.Setup(obj => obj.GetAuthor(It.IsAny<Func<AuthorDTO, bool>>())).ReturnsAsync(new AuthorDTO());
            likeServiceMock.Setup(obj => obj.LikeCard(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

            ViewResult result = await controller.Like(10) as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task Details_ParameterIsNull_ReturnsBadRequestStatusCode()
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            HttpStatusCodeResult result = await controller.Details(null) as HttpStatusCodeResult;

            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [Test()]
        public async Task Details_CardIsNotExist_ReturnsNotFoundStatusCode()
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            cardServiceMock.Setup(obj => obj.GetCard(It.IsAny<int>())).Throws(new ObjectNotFoundException(""));

            HttpStatusCodeResult result = await controller.Details(100) as HttpStatusCodeResult;

            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [Test()]
        public async Task Create_CardIsUnique_ReturnsRedirectToAction()
        {
            string expected = "Details";
            authorServiceMock.Setup(obj => obj.GetAuthor(It.IsAny<Func<AuthorDTO, bool>>())).ReturnsAsync(new AuthorDTO() { Id = 1 });
            cardServiceMock.Setup(obj => obj.CreateCard(It.IsAny<CardDTO>())).ReturnsAsync(new CardDTO());

            RedirectToRouteResult result = await controller.Create(new CardViewModel()) as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.RouteValues["action"]);
        }

        [Test()]
        public async Task Create_CardIsNotUnique_ReturnsView()
        {
            string expected = "Create";
            authorServiceMock.Setup(obj => obj.GetAuthor(It.IsAny<Func<AuthorDTO, bool>>())).ReturnsAsync(new AuthorDTO() { Id = 1 });
            cardServiceMock.Setup(obj => obj.CreateCard(It.IsAny<CardDTO>())).Throws(new ValidationException("", ""));

            ViewResult result = await controller.Create(new CardViewModel()) as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task Create_ModelIsNotValid_ReturnsView()
        {
            string expected = "Create";

            controller.ModelState.AddModelError("", "");
            ViewResult result = await controller.Create(new CardViewModel()) as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(expected, result.ViewName);
        }

        [Test()]
        public async Task Edit_ParameterIsNull_ReturnBadRequestStatusCode()
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            HttpStatusCodeResult result = await controller.Edit(new Nullable<int>()) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [Test()]
        public async Task Delete_ParameterIsNull_ReturnBadRequestStatusCode()
        {
            var expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            HttpStatusCodeResult result = await controller.Delete(new int?()) as HttpStatusCodeResult;

            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

    }
}
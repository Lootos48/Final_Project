using CardFile.BLL.DTO;
using CardFile.BLL.Infrastructure;
using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CardFile.BLL.Services.Tests
{
    [TestFixture()]
    public class IdentityServiceTests
    {
        List<IdentityUser> testUsers = new List<IdentityUser>();
        List<string> testRoles;

        readonly Mock<IIdentityProvider> identityProviderMock = new Mock<IIdentityProvider>();
        IdentityService identityService;

        [SetUp]
        public void Setup()
        {
            testUsers = new List<IdentityUser>();
            testRoles = new List<string>() { "RegiteredUser", "Admin" };

            identityService = new IdentityService(identityProviderMock.Object);
        }

        public static bool AreEqualByJson(object expected, object actual)
        {
            var serializer = new JavaScriptSerializer();
            var expectedJson = serializer.Serialize(expected);
            var actualJson = serializer.Serialize(actual);
            return expectedJson == actualJson;
        }


        [Test()]
        public async Task CreateUser_UniqueUser_ReturnsTrue()
        {
            identityProviderMock
                .Setup(obj => obj.CreateUser(It.IsAny<UserAuthInfo>()))
                .ReturnsAsync(IdentityResult.Success);

            identityProviderMock.Setup(obj => obj.GiveRoleToUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var actual = await identityService.CreateUser(new DTO.UserAuthInfoDTO() { Username = "", Password = "" });

            Assert.IsTrue(actual);
        }

        [Test()]
        public async Task CreateUser_NonUniqueUser_ThrowsException()
        {
            identityProviderMock
                .Setup(obj => obj.CreateUser(It.IsAny<UserAuthInfo>()))
                .ReturnsAsync(new IdentityResult("Error"));

            Assert.ThrowsAsync<ValidationException>(() => identityService.CreateUser(new UserAuthInfoDTO()));
        }

        [Test()]
        public void GetUsers_ReturnsAllEntities()
        {
            testUsers.AddRange(new List<IdentityUser>() { new IdentityUser(), new IdentityUser() });

            identityProviderMock
                .Setup(obj => obj.GetUsers()).Returns(testUsers);
            int expectedCount = 2;

            var actual = identityService.GetUsers();

            Assert.AreEqual(expectedCount, actual.Count());
        }

        [Test()]
        public void GetRoles_ReturnsAllEntities()
        {
            identityProviderMock
                .Setup(obj => obj.GetRoles()).Returns(testRoles);
            int expectedCount = 2;

            var actual = identityService.GetRoles();

            Assert.AreEqual(expectedCount, actual.Count);
        }

        [Test()]
        public async Task GetUserRoles_CorrectWork()
        {
            identityProviderMock
                .Setup(obj => obj.GetUserRoles(It.IsAny<string>())).ReturnsAsync(new List<string>() { testRoles[0] });
            var expected = new List<string>() { testRoles[0] };

            var actual = await identityService.GetUserRoles("User");

            Assert.AreEqual(expected, actual);
        }

        [Test()]
        public async Task CreateRole_CorrectWork()
        {
            identityProviderMock
                .Setup(obj => obj.CreateRole(It.IsAny<string>())).ReturnsAsync(() =>
                {
                    testRoles.Add("test");
                    return true;
                });
            int expectedCount = 3;

            await identityService.CreateRole("test");
            int actual = testRoles.Count;

            Assert.AreEqual(expectedCount, actual);
        }

        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public async Task<bool> GiveRoleToUser_CorrectWork(bool providerOperationResult)
        {
            identityProviderMock
                .Setup(obj => obj.GiveRoleToUser(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(providerOperationResult);

            return await identityService.GiveRoleToUser("role", "user");
        }

        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public async Task<bool> RemoveUserFromRole_CorrectWork(bool providerOperationResult)
        {
            identityProviderMock
                .Setup(obj => obj.RemoveUserFromRole(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(providerOperationResult);

            return await identityService.RemoveUserFromRole("role", "user");
        }

        [TestCase(true, ExpectedResult = true)]
        [TestCase(false, ExpectedResult = false)]
        public async Task<bool> RemoveUserFromAllRoles_CorrectWork(bool providerOperationResult)
        {
            identityProviderMock
                .Setup(obj => obj.RemoveUserFromAllRoles(It.IsAny<string>())).ReturnsAsync(providerOperationResult);

            return await identityService.RemoveUserFromAllRoles("user");
        }

        [Test()]
        public async Task GetUserClaims_UserAuthInfoExist_ReturnsClaims()
        {
            identityProviderMock
                .Setup(obj => obj.GetUserClaim(It.IsAny<UserAuthInfo>())).ReturnsAsync(new ClaimsIdentity("Google"));
            var expected = new ClaimsIdentity("Google");

            var actual = await identityService.GetUserClaims(new UserAuthInfoDTO());

            Assert.IsTrue(AreEqualByJson(expected, actual));
        }

        [Test()]
        public void GetUserClaims_UserAuthInfoNotExist_ThrowsException()
        {
            identityProviderMock
                .Setup(obj => obj.GetUserClaim(It.IsAny<UserAuthInfo>()));

            Assert.ThrowsAsync<ValidationException>(() => identityService.GetUserClaims(new UserAuthInfoDTO()));
        }
    }
}
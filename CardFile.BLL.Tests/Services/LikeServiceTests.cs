using NUnit.Framework;
using CardFile.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardFile.DAL.Interfaces;
using Moq;

namespace CardFile.BLL.Services.Tests
{
    [TestFixture()]
    public class LikeServiceTests
    {
        private readonly Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
        private readonly Mock<ILikeRepository> cardRepositoryMock = new Mock<ILikeRepository>();
        private LikeService likeService;

        [SetUp]
        public void Setup()
        {
            uowMock.Setup(obj => obj.Likes).Returns(cardRepositoryMock.Object);
            likeService = new LikeService(uowMock.Object);
        }

        [TestCase(false, ExpectedResult = false)]
        [TestCase(true, ExpectedResult = true)]
        public bool IsAuthorAlreadyLikeCard_CorrectWork(bool alreadyLikedCard)
        {
            cardRepositoryMock.Setup(obj => obj.IsAuthorAlreadyLikeCard(It.IsAny<int>(), It.IsAny<int>())).Returns(alreadyLikedCard);

            return likeService.IsAuthorAlreadyLikeCard(10, 10);
        }

        [TestCase(false, ExpectedResult = false)]
        [TestCase(true, ExpectedResult = true)]
        public async Task<bool> LikeCard_CorrectWork(bool alreadyLikedCard)
        {
            cardRepositoryMock.Setup(obj => obj.LikeCard(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(alreadyLikedCard);

            return await likeService.LikeCard(10, 10);
        }
    }
}
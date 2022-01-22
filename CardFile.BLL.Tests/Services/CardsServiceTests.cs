using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;
using Moq;
using CardFile.BLL.DTO;
using System.Web.Script.Serialization;
using CardFile.BLL.Infrastructure;

namespace CardFile.BLL.Services.Tests
{
    [TestFixture()]
    public class CardsServiceTests
    {
        private List<Card> testCards = new List<Card>();
        private List<CardDTO> testCardsDTO = new List<CardDTO>();
        private CardsService cardService;

        private readonly Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
        private readonly Mock<IRepository<Card>> cardRepositoryMock = new Mock<IRepository<Card>>();

        [SetUp]
        public void Setup()
        {
            testCards = new List<Card>();
            testCardsDTO = new List<CardDTO>();

            cardRepositoryMock.Setup(obj => obj.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Card>)testCards));

            uowMock.Setup(obj => obj.Cards).Returns(cardRepositoryMock.Object);
            cardService = new CardsService(uowMock.Object);
        }

        public static bool AreEqualByJson(object expected, object actual)
        {
            var serializer = new JavaScriptSerializer();
            var expectedJson = serializer.Serialize(expected);
            var actualJson = serializer.Serialize(actual);
            return expectedJson == actualJson;
        }

        [Test()]
        public async Task GetAllAsync_Returns_IEnumerableCardDTO()
        {
            IEnumerable<CardDTO> expected = new List<CardDTO>();

            var actual = await cardService.GetAll();

            Assert.IsInstanceOf(expected.GetType(), actual);
        }

        [Test()]
        public async Task GetAll_ReturnsAllEnitites()
        {
            testCards.AddRange(new List<Card>() { new Card { Id = 1 }, new Card { Id = 2 } });
            testCardsDTO.AddRange(new List<CardDTO>() { new CardDTO { Id = 1 }, new CardDTO { Id = 2 } });
            IEnumerable<CardDTO> expected = testCardsDTO;


            IEnumerable<CardDTO> actual = await cardService.GetAll();

            Assert.AreEqual(expected.Count(), actual.Count());
        }

        [TestCase("Test1")]
        public async Task CreateCard_CorrectWork(string title)
        {
            testCards.AddRange(new List<Card>() { new Card { Id = 1, Title = "Test" } });
            CardDTO expected = new CardDTO() { Id = 2, Title = title };

            uowMock.Setup(obj => obj.Cards.CreateAsync(It.IsAny<Card>())).ReturnsAsync(new Card() { Id = 2, Title = title });

            var actual = await cardService.CreateCard(new CardDTO());

            Assert.IsTrue(AreEqualByJson(expected, actual));
        }

        [TestCase("Test")]
        public async Task CreateCard_AddingCardWithTitleThatExist_ThrowException(string title)
        {
            testCards.AddRange(new List<Card>() { new Card { Id = 1, Title = title } });
            CardDTO expected = new CardDTO() { Id = 2, Title = title };

            Assert.ThrowsAsync<ValidationException>(() => cardService.CreateCard(expected));
        }

        [TestCase(1)]
        public async Task GetCard_CardIsExist_CorrectWork(int id)
        {
            testCards.Add(new Card() { Id = id });
            CardDTO expected = new CardDTO() { Id = id };

            uowMock.Setup(obj => obj.Cards.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(testCards
                    .SingleOrDefault(x => x.Id == id));

            var actual = await cardService.GetCard(id);

            Assert.IsTrue(AreEqualByJson(expected, actual));
        }

        [TestCase(2)]
        public void GetCard_CardIsNotExist_ThrowsException(int id)
        {
            testCards.Add(new Card() { Id = 1 });

            uowMock.Setup(obj => obj.Cards.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(testCards
                    .SingleOrDefault(x => x.Id == id));

            Assert.ThrowsAsync<ObjectNotFoundException>(() => cardService.GetCard(id));
        }

        [TestCase("Test")]
        [TestCase("Test1")]
        public async Task UpdateCard_CorrectWork(string title)
        {
            testCards.Add(new Card() { Id = 1, Title = "Test" });
            uowMock.Setup(obj => obj.Cards.UpdateAsync(It.IsAny<Card>())).ReturnsAsync(true);

            Assert.IsTrue(await cardService.UpdateCard(new CardDTO() { Id = 1, Title = title }));
        }

        [TestCase(1, "Test2")]
        public void UpdateCard_TitleIsAlreadyExist_ThrowsValidationException(int id, string title)
        {
            testCards.AddRange(new List<Card>() { new Card() { Id = 1, Title = "Test1" }, new Card() { Id = 2, Title = "Test2" } });
            uowMock.Setup(obj => obj.Cards.UpdateAsync(It.IsAny<Card>())).ReturnsAsync(false);

            Assert.ThrowsAsync<ValidationException>(() => cardService.UpdateCard(new CardDTO() { Id = id, Title = title }));
        }

        [TestCase(100, "Test1000")]
        public void UpdateCard_TitleIsAlreadyExist_ThrowsNotFoundException(int id, string title)
        {
            testCards.AddRange(new List<Card>() { new Card() { Id = 1, Title = "Test1" }, new Card() { Id = 2, Title = "Test2" } });
            uowMock.Setup(obj => obj.Cards.UpdateAsync(It.IsAny<Card>())).ReturnsAsync(false);

            Assert.ThrowsAsync<ObjectNotFoundException>(() => cardService.UpdateCard(new CardDTO() { Id = id, Title = title }));
        }

        [TestCase(1, true)]
        public async Task DeleteCard_CardIsExist_CorrectWorks(int id, bool isDeleted)
        {
            testCards.Add(new Card() { Id = 1 });
            uowMock.Setup(obj => obj.Cards.RemoveAsync(It.IsAny<int>())).ReturnsAsync(isDeleted);

            Assert.IsTrue(await cardService.DeleteCard(id));
        }

        [TestCase(10, false)]
        public async Task DeleteCard_CardIsNotExist_ThrowsException(int id, bool isDeleted)
        {
            testCards.Add(new Card() { Id = 1 });
            uowMock.Setup(obj => obj.Cards.RemoveAsync(It.IsAny<int>())).ReturnsAsync(isDeleted);

            Assert.ThrowsAsync<ObjectNotFoundException>(() => cardService.DeleteCard(id));
        }
    }
}
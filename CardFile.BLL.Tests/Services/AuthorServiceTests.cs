using CardFile.BLL.Services;
using CardFile.BLL.DTO;
using CardFile.BLL.Infrastructure;
using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CardFile.BLL.Services.Tests
{
    [TestFixture()]
    public class AuthorServiceTests
    {
        private List<Author> testAuthors = new List<Author>();
        private List<AuthorDTO> testAuthorsDTO = new List<AuthorDTO>();
        private AuthorService authorService;

        private readonly Mock<IUnitOfWork> uowMock = new Mock<IUnitOfWork>();
        private readonly Mock<IRepository<Author>> authorRepositoryMock = new Mock<IRepository<Author>>();

        [SetUp]
        public void Startup()
        {
            testAuthors = new List<Author>();
            testAuthorsDTO = new List<AuthorDTO>();

            authorRepositoryMock.Setup(obj => obj.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Author>)testAuthors));

            uowMock.Setup(obj => obj.Authors).Returns(authorRepositoryMock.Object);
            authorService = new AuthorService(uowMock.Object);
        }

        public static bool AreEqualByJson(object expected, object actual)
        {
            var serializer = new JavaScriptSerializer();
            var expectedJson = serializer.Serialize(expected);
            var actualJson = serializer.Serialize(actual);
            return expectedJson == actualJson;
        }

        [Test()]
        public async Task CreateAuthor_CorrectWork()
        {
            var expected = new AuthorDTO() { Id = 1 };
            uowMock.Setup(obj => obj.Authors.CreateAsync(It.IsAny<Author>())).ReturnsAsync(new Author() { Id = 1 });

            var actual = await authorService.CreateAuthor(expected);

            Assert.IsTrue(AreEqualByJson(expected, actual));
        }

        [TestCase(1)]
        public async Task GetAuthor_AuthorExist_ReturnsAuthor(int id)
        {
            testAuthors.Add(new Author() { Id = 1 });
            var expected = new AuthorDTO() { Id = 1 };
            uowMock.Setup(obj => obj.Authors.FindByIdAsync(id)).ReturnsAsync(testAuthors.SingleOrDefault(x => x.Id == id));

            var actual = await authorService.GetAuthor(id);

            Assert.IsTrue(AreEqualByJson(expected, actual));
        }

        [TestCase(100)]
        public void GetAuthor_AuthorNotExist_ThrowsException(int id)
        {
            testAuthors.Add(new Author() { Id = 1 });
            uowMock.Setup(obj => obj.Authors.FindByIdAsync(id)).ReturnsAsync(testAuthors.SingleOrDefault(x => x.Id == id));

            Assert.ThrowsAsync<ObjectNotFoundException>(() => authorService.GetAuthor(id));
        }

        [Test()]
        public async Task GetAll_ReturnsAllEntites()
        {
            testAuthors.AddRange(new List<Author>() { new Author() { Id = 1 }, new Author() { Id = 2 }, new Author() { Id = 3 } });
            var expectedCount = 3;

            Assert.AreEqual(expectedCount, (await authorService.GetAll()).Count());
        }

        [Test()]
        public async Task UpdateAuthor_AuthorExist_ReturnsTrue()
        {
            uowMock.Setup(obj => obj.Authors.UpdateAsync(It.IsAny<Author>())).ReturnsAsync(true);

            var actual = await authorService.UpdateAuthor(new AuthorDTO());

            Assert.IsTrue(actual);
        }

        [Test()]
        public async Task UpdateAuthor_AuthorNotExist_ThrowsException()
        {
            uowMock.Setup(obj => obj.Authors.UpdateAsync(It.IsAny<Author>())).ReturnsAsync(false);

            Assert.ThrowsAsync<ObjectNotFoundException>(() => authorService.UpdateAuthor(new AuthorDTO()));
        }

        [Test()]
        public async Task DeleteAuthor_AuthorExist_ReturnsTrue()
        {
            uowMock.Setup(obj => obj.Authors.RemoveAsync(It.IsAny<int>())).ReturnsAsync(true);

            var actual = await authorService.DeleteAuthor(1);

            Assert.IsTrue(actual);
        }

        [Test()]
        public async Task DeleteAuthor_AuthorNotExist_ThrowsException()
        {
            uowMock.Setup(obj => obj.Authors.RemoveAsync(It.IsAny<int>())).ReturnsAsync(false);

            Assert.ThrowsAsync<ObjectNotFoundException>(() => authorService.DeleteAuthor(1));
        }

        
    }
}
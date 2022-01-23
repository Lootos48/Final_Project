using CardFile.BLL.Interfaces;
using CardFile.DAL.EF;
using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading.Tasks;

namespace CardFile.BLL.Services
{
    /// <summary>
    /// Метод реализации интерфейса <see cref="ILikeService"/>
    /// <inheritdoc cref="ILikeService"/>
    /// </summary>
    public class LikeService : ILikeService
    {
        /// <summary>
        /// Поле для работы с контекстом 
        /// </summary>
        private readonly IUnitOfWork Database;

        /// <summary>
        /// Конструктор в котором инициализируется поле контекта БД
        /// </summary>
        /// <param name="context">Контекст БД который передаётся в конструктор с помощью IoC-контейнера</param>
        public LikeService(IUnitOfWork uow)
        {
            this.Database = uow;
        }
        public bool IsAuthorAlreadyLikeCard(int cardId, int authorId)
        {
            return Database.Likes.IsAuthorAlreadyLikeCard(cardId, authorId);
        }
        public async Task<bool> LikeCard(int cardId, int authorId)
        {
            return await Database.Likes.LikeCard(cardId, authorId);
        }
    }
}

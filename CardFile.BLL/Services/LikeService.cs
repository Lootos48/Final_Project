using CardFile.BLL.Interfaces;
using CardFile.DAL.EF;
using CardFile.DAL.Entities;
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
        private readonly CardFileContext context;

        /// <summary>
        /// Конструктор в котором инициализируется поле контекта БД
        /// </summary>
        /// <param name="context">Контекст БД который передаётся в конструктор с помощью IoC-контейнера</param>
        public LikeService(CardFileContext context)
        {
            this.context = context;
        }
        public bool IsAuthorAlreadyLikeCard(int cardId, int authorId)
        {
            return context.AuthorsLikedCards.Any(alc => alc.CardId == cardId && alc.AuthorId == authorId);
        }
        public async Task<bool> LikeCard(int cardId, int authorId)
        {
            if (context.AuthorsLikedCards.Any(alc => alc.CardId == cardId && alc.AuthorId == authorId))
            {
                return false;
            }
            AuthorsLikedCards authorLikesCards = new AuthorsLikedCards()
            {
                CardId = cardId,
                AuthorId = authorId
            };
            context.AuthorsLikedCards.Add(authorLikesCards);
            var card = context.Cards.First(c => c.Id == cardId);
            if (card != null)
            {
                card.LikeAmount++;
                context.Entry(card).CurrentValues.SetValues(card);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

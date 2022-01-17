using CardFile.BLL.Interfaces;
using CardFile.DAL.EF;
using CardFile.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace CardFile.BLL.Services
{
    public class LikeService : ILikeService
    {
        private readonly CardFileContext context;
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

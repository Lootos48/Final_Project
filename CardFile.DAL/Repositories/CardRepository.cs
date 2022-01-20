using CardFile.DAL.EF;
using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Repositories
{
    /// <inheritdoc cref="IRepository{TEntity}"/>
    public class CardRepository : IRepository<Card>
    {
        /// <summary>
        /// Поле для работы с классом контекста БД
        /// </summary>
        readonly CardFileContext _context;

        /// <summary>
        /// Конструктор класса который передаёт контекст БД репозиторию
        /// </summary>
        /// <param name="context">Объект класса контекста БД</param>
        public CardRepository(CardFileContext context)
        {
            _context = context;
        }

        public async Task<Card> CreateAsync(Card item)
        {
            _context.Cards.Add(item);
            /*await _context.SaveChangesAsync();*/

            return item;
        }

        public async Task<Card> FindByIdAsync(int id)
        {
            var card = await _context.Cards.Include(c => c.Author).SingleOrDefaultAsync(x => x.Id == id);
            return card;
        }

        public async Task<IEnumerable<Card>> GetAllAsync()
        {
            var cards = await _context.Cards.Include(c => c.Author).ToListAsync();
            return cards;
        }

        public async Task<IEnumerable<Card>> GetAsync(Func<Card, bool> predicate)
        {
            return await _context.Cards.Where(predicate).AsQueryable().ToListAsync();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var item = _context.Cards.Find(id);
            var result = item != null;
            if (result)
            {
                _context.Entry(item).State = EntityState.Deleted;
                /*await _context.SaveChangesAsync();*/
            }

            return result;
        }

        public async Task<bool> UpdateAsync(Card item)
        {
            var entity = _context.Cards.Find(item.Id);
            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(item);
                /*await _context.SaveChangesAsync();*/
                return true;
            }
            return false;
        }
    }
}

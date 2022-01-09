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
    public class CardRepository : IRepository<Card>
    {
        readonly CardFileContext _context;

        public CardRepository(CardFileContext context)
        {
            _context = context;
        }

        public async Task<Card> CreateAsync(Card item)
        {
            _context.Cards.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task<Card> FindByIdAsync(int id)
        {
            return await _context.Cards.FindAsync(id);
        }

        public async Task<IEnumerable<Card>> GetAllAsync()
        {
            return await _context.Cards.ToListAsync();
        }

        public async Task<IEnumerable<Card>> GetAsync(Func<Card, bool> predicate)
        {
            return await _context.Cards.Where(predicate).AsQueryable().ToListAsync();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var item = _context.Cards.Attach(new Card { Id = id });
            var result = item != null;
            if (result)
            {
                _context.Entry(item).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<bool> UpdateAsync(Card item)
        {
            _context.Entry(item).State = EntityState.Modified;

            int rowsAffected = await _context.SaveChangesAsync();

            if (rowsAffected == 0)
            {
                return false;
            }

            return true;
        }
    }
}

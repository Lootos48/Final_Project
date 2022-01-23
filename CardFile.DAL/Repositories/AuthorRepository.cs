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
    public class AuthorRepository : IRepository<Author>
    {
        /// <summary>
        /// Поле для работы с классом контекста БД
        /// </summary>
        readonly CardFileContext _context;

        /// <summary>
        /// Конструктор класса который передаёт текущий контекст БД в репозиторий
        /// </summary>
        /// <param name="context">Объект класса контекста БД</param>
        public AuthorRepository(CardFileContext context)
        {
            _context = context;
        }

        public async Task<Author> CreateAsync(Author item)
        {
            _context.Authors.Add(item);
            return item;
        }

        public async Task<Author> FindByIdAsync(int id)
        {
            return await _context.Authors.Include(x => x.Cards).SingleOrDefaultAsync(y => y.Id == id);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.Include(a => a.Cards).ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetAsync(Func<Author, bool> predicate)
        {
            return await _context.Authors.Where(predicate).AsQueryable().ToListAsync();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var item = _context.Authors.Find(id);
            var result = item != null;
            if (result)
            {
                _context.Entry(item).State = EntityState.Deleted;
            }

            return result;
        }

        public async Task<bool> UpdateAsync(Author item)
        {
            var entity = _context.Authors.Include(c => c.Cards).Single(a => a.Id == item.Id);
            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(item);
                return true;
            }
            return false;
        }
    }
}

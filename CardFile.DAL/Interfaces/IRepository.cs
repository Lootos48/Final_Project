using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity item);

        Task<TEntity> FindByIdAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate);

        Task<bool> RemoveAsync(int id);

        Task<bool> UpdateAsync(TEntity item);
    }
}

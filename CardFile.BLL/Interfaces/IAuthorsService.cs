using CardFile.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    /// <summary>
    /// Интерфейс для реализации вызова CRUD-операций репозитория
    /// </summary>
    public interface IAuthorsService : IDisposable
    {
        
        Task<AuthorDTO> CreateAuthor(AuthorDTO authorDto);
        Task<AuthorDTO> GetAuthor(int? id);
        Task<IEnumerable<AuthorDTO>> GetAll();
        Task<bool> UpdateAuthor(AuthorDTO authorDTO);
        Task<bool> DeleteAuthor(int authorDTO);
    }
}

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
        /// <summary>
        /// Метод для создания автора
        /// </summary>
        /// <param name="authorDto">Создаваемая сущность автора</param>
        /// <returns>Созданная сущность автора <see cref="AuthorDTO"/></returns>
        Task<AuthorDTO> CreateAuthor(AuthorDTO authorDto);

        /// <summary>
        /// Метод для получения автора по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор искомого автора</param>
        /// <returns>Объект <see cref="AuthorDTO"/> с искомым идентификатором</returns>
        Task<AuthorDTO> GetAuthor(int? id);

        /// <summary>
        /// Метод для получения экземпляра автора
        /// </summary>
        /// <param name="predicate">Условие выбора автора</param>
        /// <returns><see cref="AuthorDTO"/> который соответствует предикату</returns>
        Task<AuthorDTO> GetAuthor(Func<AuthorDTO, bool> predicate);

        /// <summary>
        /// Метод для получения всех существующих авторов
        /// </summary>
        /// <returns><see cref="IEnumerable{AuthorDTO}"/> коллекцию объектов <see cref="AuthorDTO"/></returns>
        Task<IEnumerable<AuthorDTO>> GetAll();

        /// <summary>
        /// Метод для редактирования автора
        /// </summary>
        /// <param name="authorDTO">Редактируемая сущность <see cref="AuthorDTO"/></param>
        /// <returns><see langword="true"/> - если операция была успешна</returns>
        Task<bool> UpdateAuthor(AuthorDTO authorDTO);

        /// <summary>
        /// Метод для удаления автора
        /// </summary>
        /// <param name="id">Идентификатор удаляемого автора</param>
        /// <returns><see langword="true"/> - если операция была успешна</returns>
        Task<bool> DeleteAuthor(int id);
    }
}

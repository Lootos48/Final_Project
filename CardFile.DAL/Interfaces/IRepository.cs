using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Interfaces
{
    /// <summary>
    /// Шаблонный репозиторий для CRUD-операций по взаимодействию с БД
    /// </summary>
    /// <typeparam name="TEntity">Сущность с которой будут производиться CRUD-операции</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Асинхронный метод для добавления сущности в контекст БД 
        /// </summary>
        /// <param name="item">Сущность которая была добавлена в контекст БД</param>
        /// <returns>Возвращает добавленную в контекст БД сущность</returns>
        Task<TEntity> CreateAsync(TEntity item);

        /// <summary>
        /// Асинхронный метод для поиска сущности по ID в контексте БД
        /// </summary>
        /// <param name="id">Искомый идентификатор сущности</param>
        /// <returns>Сущность с искомым идентификатором</returns>
        Task<TEntity> FindByIdAsync(int id);

        /// <summary>
        /// Асинхронный метод для получения коллекции всех сущностей в контексте БД
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Асинхронный метод для получения сущности что соответствует переданному предикату
        /// </summary>
        /// <param name="predicate">Предикат для филтрации выборки</param>
        /// <returns>Коллекцию сущностей что соответствует переданному предикату</returns>
        Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate);

        /// <summary>
        /// Асинхронный метод для удаления сущности из БД
        /// </summary>
        /// <param name="id">Идентификатор искомой сущности</param>
        /// <returns>True - если сущность была удалена, False - если нет</returns>
        Task<bool> RemoveAsync(int id);

        /// <summary>
        /// Асинхронный метод для изменения сущности в БД
        /// </summary>
        /// <param name="item">Сущность которую нужно поменять</param>
        /// <returns>True - если сущность была обновлена, False - если нет</returns>
        Task<bool> UpdateAsync(TEntity item);
    }
}

using CardFile.BLL.DTO;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    /// <summary>
    /// Интерфейс CRUD-операций с пользователями
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Метод получения всех существующих пользователей
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> коллекцию объектов класса <seealso cref="IdentityUser"/></returns>
        IEnumerable<IdentityUser> GetUsers();

        /// <summary>
        /// Метод для получения всех существующих ролей
        /// </summary>
        /// <returns>Коллецию <see cref="List{T}"/> типа <seealso langword="string"/> с названиями ролей</returns>
        List<string> GetRoles();

        /// <summary>
        /// Метод для получения всех ролей определённого пользователя
        /// </summary>
        /// <param name="username">Никнейм пользователя</param>
        /// <returns>оллецию <see cref="List{T}"/> типа <seealso langword="string"/> с названиями ролей этого пользователя</returns>
        Task<IList<string>> GetUserRoles(string username);

        /// <summary>
        /// Метод для создания нового пользователя
        /// </summary>
        /// <param name="user">Данные для аутентификации пользователя</param>
        /// <returns><see langword="true"/> - если операци была успешна</returns>
        Task<bool> CreateUser(UserAuthInfoDTO user);

        /// <summary>
        /// Метод для создания новой роли
        /// </summary>
        /// <param name="role">Название новой роли</param>
        /// <returns><see langword="true"/> - если операци была успешна</returns>
        Task<bool> CreateRole(string role);

        /// <summary>
        /// Метод для присваивания пользователю новой роли
        /// </summary>
        /// <param name="role">Название роли</param>
        /// <param name="username">Никнейм пользователя</param>
        /// <returns><see langword="true"/> - если операци была успешна</returns>
        Task<bool> GiveRoleToUser(string role, string username);

        /// <summary>
        /// Метод для отнимания у пользователя определённой роли
        /// </summary>
        /// <param name="role">Название роли</param>
        /// <param name="username">Никнейм пользователя</param>
        /// <returns><see langword="true"/> - если операци была успешна</returns>
        Task<bool> RemoveUserFromRole(string username, string role);

        /// <summary>
        /// Метод для отнимания у пользователя всех ролей
        /// </summary>
        /// <param name="username">Никнейм пользователя</param>
        /// <returns><see langword="true"/> - если операци была успешна</returns>
        Task<bool> RemoveUserFromAllRoles(string username);

        /// <summary>
        /// Метод для получения данных аутентификации пользователя
        /// </summary>
        /// <param name="user">Данные аутентификации пользователя </param>
        /// <returns>Объект класса <see cref="ClaimsIdentity"/></returns>
        Task<ClaimsIdentity> GetUserClaims(UserAuthInfoDTO user);
    }
}

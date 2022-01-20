using CardFile.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CardFile.DAL.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с Identity
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        /// Метод для получения всех имеющихся в БД пользователей
        /// </summary>
        /// <returns>Коллекция пользователей типа IdentityUser</returns>
        IEnumerable<IdentityUser> GetUsers();

        /// <summary>
        /// Метод для получения всех имеющихся на сервере ролей
        /// </summary>
        /// <returns>Строчный список имеющихся на сервере ролей</returns>
        List<string> GetRoles();

        /// <summary>
        /// Метод для получения всех ролей определённого пользователя
        /// </summary>
        /// <param name="username">Никнейм искомого пользователя</param>
        /// <returns>Список всех ролей пользователя</returns>
        Task<IList<string>> GetUserRoles(string username);

        /// <summary>
        /// Метод для создания нового пользователя
        /// </summary>
        /// <param name="user">Данные для аутентификации пользователя</param>
        /// <returns><see cref="IdentityResult"/> для определения успешности операции</returns>
        Task<IdentityResult> CreateUser(UserAuthInfo user);

        /// <summary>
        /// Метод для создания новой роли
        /// </summary>
        /// <param name="role">Название новой роли</param>
        /// <returns><see langword="true"/> если операция была успешна</returns>
        Task<bool> CreateRole(string role);

        /// <summary>
        /// Метод для присваивания пользователю новой роли 
        /// </summary>
        /// <param name="role">Присваиваемая роль</param>
        /// <param name="username">Никнейм пользователя</param>
        /// <returns><see langword="true"/> если операция бьла успешна</returns>
        Task<bool> GiveRoleToUser(string role, string username);

        /// <summary>
        /// Метод для удаления пользователю определённой роли 
        /// </summary>
        /// <param name="role">Удаляемая роль</param>
        /// <param name="username">Никнейм пользователя</param>
        /// <returns><see langword="true"/> если операция бьла успешна</returns>
        Task<bool> RemoveUserFromRole(string username, string role);

        /// <summary>
        /// Метод для удаления пользователя из всех ролей в которых он состоит
        /// </summary>
        /// <param name="username">Никнейм пользователя</param>
        /// <returns><see langword="true"/> если операция бьла успешна</returns>
        Task<bool> RemoveUserFromAllRoles(string username);
        
        /// <summary>
        /// Метод для получения входных данных пользователя
        /// </summary>
        /// <param name="user">Данные для аутентификации пользователя</param>
        /// <returns>Возвращает объект класса <see cref="ClaimsIdentity"/></returns>
        Task<ClaimsIdentity> GetUserClaim(UserAuthInfo user);
    }
}

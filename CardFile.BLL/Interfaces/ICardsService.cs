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
    public interface ICardsService : IDisposable
    {
        /// <summary>
        /// Метод для создания карточки
        /// </summary>
        /// <param name="cardDto">Создаваемая сущность карточки</param>
        /// <returns>Созданная сущность карточки <see cref="CardDTO"/></returns>
        Task<CardDTO> CreateCard(CardDTO cardDto);

        /// <summary>
        /// Метод для получения карточки по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор искомой карточки</param>
        /// <returns>Объект <see cref="CardDTO"/> с искомым идентификатором</returns>
        Task<CardDTO> GetCard(int? id);

        /// <summary>
        /// Метод для получения всех существующих карточек
        /// </summary>
        /// <returns><see cref="IEnumerable{CardDTO}"/> коллекцию объектов <see cref="CardDTO"/></returns>
        Task<IEnumerable<CardDTO>> GetAll();


        /// <summary>
        /// Метод для редактирования карточки
        /// </summary>
        /// <param name="cardDTO">Редактируемая сущность <see cref="CardDTO"/></param>
        /// <returns><see langword="true"/> - если операция была успешна</returns>
        Task<bool> UpdateCard(CardDTO cardDTO);


        /// <summary>
        /// Метод для удаления карточки
        /// </summary>
        /// <param name="id">Идентификатор удаляемой сущности</param>
        /// <returns><see langword="true"/> - если операция была успешна</returns>
        Task<bool> DeleteCard(int id);
    }
}

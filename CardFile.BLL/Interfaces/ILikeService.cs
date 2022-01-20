using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Interfaces
{
    /// <summary>
    /// Интерфейс с методами по работе с системой оценок
    /// </summary>
    public interface ILikeService
    {
        /// <summary>
        /// Метод для проверки оценил ли пользователь карточку
        /// </summary>
        /// <param name="cardId">Идентификатор карточки которой пользователь поставил оценку</param>
        /// <param name="authorId">Идентификатор пользователя который поставил оценку</param>
        /// <returns><see langword="true"/> - если пользователь оценил карточку, <seealso langword="false"/> - если нет</returns>
        bool IsAuthorAlreadyLikeCard(int cardId, int authorId);

        /// <summary>
        /// Метод для занесения оценки карточки автором
        /// </summary>
        /// <param name="cardId">Идентификатор карточки которой поставили оценку</param>
        /// <param name="authorId">Идентификатор автора который поставил оценку</param>
        /// <returns><see langword="true"/> - если операция была успешна</returns>
        Task<bool> LikeCard(int cardId, int authorId);
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardFile.DAL.Entities
{
    /// <summary>
    /// Модель для хранения информации о поставленном лайке автора - определённой карточке
    /// </summary>
    public class AuthorsLikedCards
    {
        /// <summary>
        /// Идентификатор автора
        /// </summary>
        [Key]
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        /// <summary>
        /// Идентификатор карточки
        /// </summary>
        [Key]
        [ForeignKey(nameof(Card))]
        public int CardId { get; set; }

        /// <summary>
        /// Навигационные свойства
        /// </summary>
        public virtual Author Author { get; set; }
        public virtual Card Card { get; set; }
    }
}

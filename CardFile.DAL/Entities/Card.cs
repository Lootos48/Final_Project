using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Entities
{
    /// <summary>
    /// Модель сущности карточки
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Поле главного ключа
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Поле для сохранения названия карточки.
        /// </summary>
        /// <remarks>Поле обязательно и имеет ограничение в минимум 4 символа и максимум 100</remarks>
        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Title size must be between 4 and 100 characters")]
        public string Title { get; set; }

        /// <summary>
        /// Поле для сохранения даты создания карточки.
        /// Поле обязательно
        /// </summary>
        [Required]
        public DateTime DateOfCreate { get; set; }

        /// <summary>
        /// Поле для сохранения текста карточки.
        /// </summary>
        /// <remarks>Поле обязательно и имеет ограничение в минимум 251 символ и максимум 2000</remarks>
        [Required]
        [StringLength(2000, MinimumLength = 251, ErrorMessage = "Text minimum size is 251 characters")]
        public string Text { get; set; }

        /// <summary>
        /// Поле для сохранения количества лайков
        /// </summary>
        /// <remarks>Начальное значение лайков равно 0 по-умолчанию</remarks>
        [DefaultValue(0)]
        public int LikeAmount { get; set; }
        
        /// <summary>
        /// Идентификатор автора карточки
        /// </summary>
        public int? AuthorId { get; set; }

        /// <summary>
        /// Навигационное свойство автора карточки
        /// </summary>
        public Author Author { get; set; }
    }
}

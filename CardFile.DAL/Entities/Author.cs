using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Entities
{
    public class Author
    {
        /// <summary>
        /// Поле главного ключа
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Поле для Никнейма Автора
        /// </summary>
        [Display(Name = "Username")]
        public string Username { get; set; }

        /// <summary>
        /// Поле для Имени Автора.
        /// </summary>
        /// <remarks>Поле обязательно и имеет ограничение в минимум 3 символа и максимум 60</remarks>

        [StringLength(60, MinimumLength = 3, ErrorMessage = "First name size must be between 3 and 60 characters")]
        public string FirstName { get; set; }

        /// <summary>
        /// Поле для Фамилии Автора.
        /// </summary>
        /// <remarks>Поле обязательно и имеет ограничение в минимум 3 символа и максимум 60</remarks>

        [StringLength(60, MinimumLength = 3, ErrorMessage = "Second name size must be between 3 and 60 characters")]
        public string SecondName { get; set; }

        public virtual ICollection<Card> Cards { get; set; }

        /// <summary>
        /// Конструктор для инициализации списков связи 1-к-многим или многие-к-многим
        /// </summary>
        public Author()
        {
            Cards = new List<Card>();
        }
    }
}

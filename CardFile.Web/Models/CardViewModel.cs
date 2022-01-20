using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    /// <summary>
    /// Модель сущности карточки
    /// </summary>
    public class CardViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Card Title")]
        public string Title { get; set; }

        /// <summary>
        /// Дата создания карточки в коротком формате
        /// </summary>
        public string DateOfCreateString
        {
            get
            {
                return DateOfCreate.ToShortDateString();
            }
        }

        [Display(Name = "Creation date")]
        public DateTime DateOfCreate { get; set; }

        [Display(Name = "Card text")]
        public string Text { get; set; }

        [Display(Name = "Likes")]
        public int LikeAmount { get; set; }

        public int? AuthorId { get; set; }
        public AuthorViewModel Author { get; set; }
    }
}
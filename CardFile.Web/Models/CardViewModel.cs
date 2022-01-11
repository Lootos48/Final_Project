using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    public class CardViewModel
    {

        public int Id { get; set; }

        [Display(Name = "Card Title")]
        public string Title { get; set; }

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

        public virtual AuthorViewModel Author { get; set; }
    }
}
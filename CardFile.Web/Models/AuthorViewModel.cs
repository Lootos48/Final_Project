using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    /// <summary>
    /// Модель сущности профиля пользователя
    /// </summary>
    public class AuthorViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Second name")]
        public string SecondName { get; set; }


        public ICollection<CardViewModel> Cards { get; set; }

        public AuthorViewModel()
        {
            Cards = new List<CardViewModel>();
        }
    }
}
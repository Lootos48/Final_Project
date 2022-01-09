﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    public class AuthorViewModel
    {
        public int Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Second name")]
        public string SecondName { get; set; }

        public virtual ICollection<CardViewModel> Cards { get; set; }

        public AuthorViewModel()
        {
            Cards = new List<CardViewModel>();
        }
    }
}
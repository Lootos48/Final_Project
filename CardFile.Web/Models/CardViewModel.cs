using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    public class CardViewModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateOfCreate { get; set; }

        public string Text { get; set; }

        public int LikeAmount { get; set; }

        public virtual AuthorViewModel Author { get; set; }
    }
}
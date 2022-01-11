using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    public class PageCardsFiltrationViewModel
    {
        public SearchBy SearchType { get; set; }
        public string SearchString { get; set; }
        public SortBy SortType { get; set; }
        public SortOrder SortOrder { get; set; }
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }

    public enum SortBy
    {
        Date,
        Title,
        Like
    }

    public enum SearchBy
    {
        Title,
        Date,
        InTextPhrase
    }
}
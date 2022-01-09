using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    public class PageInfoModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }

    public class IndexViewModel<T> where T : class
    {
        public IEnumerable<T> PageObjects { get; set; }
        public PageInfoModel PageInfo { get; set; }
    }
}
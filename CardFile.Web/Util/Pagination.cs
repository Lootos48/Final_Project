using CardFile.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Util
{
    public static class Pagination<T> where T : class
    {
        public static IndexViewModel<T> PaginateObjects(IEnumerable<T> allObjects, int page, int pageSize)
        {
            IEnumerable<T> objectsPerPage = allObjects.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfoModel pageInfo = new PageInfoModel { PageNumber = page, PageSize = pageSize, TotalItems = allObjects.Count() };
            IndexViewModel<T> ivm = new IndexViewModel<T> { PageInfo = pageInfo, PageObjects = objectsPerPage };

            return ivm;
        }
    }
}
using CardFile.Web.Enums;
using CardFile.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Util
{
    /// <summary>
    /// Статический класс для составления данных о странице и элементов на текущей странице
    /// </summary>
    /// <typeparam name="T">Тип элементов на странице</typeparam>
    public static class Pagination<T> where T : class
    {
        /// <summary>
        /// Класс пагинации объектов для которых не предусмотрена сортировка и филтрация
        /// </summary>
        /// <param name="allObjects">Коллеция всех объектов</param>
        /// <param name="page">Текущий номер страницы</param>
        /// <param name="pageSize">Кол-во элементов на странице</param>
        /// <returns>Класс информации о странице и её данных</returns>
        public static IndexViewModel<T> PaginateObjects(IEnumerable<T> allObjects, int page, int pageSize)
        {
            IEnumerable<T> objectsPerPage = allObjects.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfoModel pageInfo = new PageInfoModel { PageNumber = page, PageSize = pageSize, TotalItems = allObjects.Count() };
            IndexViewModel<T> ivm = new IndexViewModel<T> { PageInfo = pageInfo, PageObjects = objectsPerPage };

            return ivm;
        }

        /// <summary>
        /// Класс пагинации объектов для которых предусмотрена сортировка и филтрация
        /// </summary>
        /// <param name="allObjects">Коллеция всех объектов</param>
        /// <param name="page">Текущий номер страницы</param>
        /// <param name="pageSize">Кол-во элементов на странице</param>
        /// <param name="pageFilter">Настройки филтрации</param>
        /// <param name="sortOptions">Порядок сортировки</param>
        /// <returns>Класс информации о странице и её данных с учётов настроек фильтрации и сортировки</returns>
        public static IndexViewModel<T> PaginateObjects(IEnumerable<T> allObjects, int page, int pageSize, PageFilter pageFilter, SortOptions sortOptions)
        {
            IEnumerable<T> objectsPerPage = allObjects.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfoModel pageInfo = new PageInfoModel { PageNumber = page, PageSize = pageSize, TotalItems = allObjects.Count(), searchFilter = pageFilter, sortOptions = sortOptions };
            IndexViewModel<T> ivm = new IndexViewModel<T> { PageInfo = pageInfo, PageObjects = objectsPerPage };

            return ivm;
        }
    }
}
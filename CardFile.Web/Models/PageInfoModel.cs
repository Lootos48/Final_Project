using CardFile.Web.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardFile.Web.Models
{
    /// <summary>
    /// Класс для информации о различных параметрах страницы
    /// </summary>
    public class PageInfoModel
    {
        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public SortOptions sortOptions { get; set; }

        /// <summary>
        /// Поле класса филтрации
        /// </summary>
        public PageFilter searchFilter { get; set; }

        /// <summary>
        /// Номер текущей страцицы
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Кол-во объектов на странице
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Общее кол-во элементов на одной страцице
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Общее кол-во страниц
        /// </summary>
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }

    /// <summary>
    /// Класс для хранения информации о параметрах страницы и элементов страницы
    /// </summary>
    /// <typeparam name="T">Тип объектов на странице</typeparam>
    public class IndexViewModel<T> where T : class
    {
        /// <summary>
        /// Коллекция объектов текущей страницы
        /// </summary>
        public IEnumerable<T> PageObjects { get; set; }

        /// <summary>
        /// Информация о странице
        /// </summary>
        public PageInfoModel PageInfo { get; set; }
    }
}
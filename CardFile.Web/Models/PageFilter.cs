using CardFile.Web.Enums;

namespace CardFile.Web.Models
{
    /// <summary>
    /// Класс для данных фильтрации карточек на странице
    /// </summary>
    public class PageFilter
    {
        /// <summary>
        /// Спопоб фильтрации
        /// </summary>
        public FilterOptions SearchBy { get; set; }
        
        /// <summary>
        /// Строка для значения поиска
        /// </summary>
        public string SearchString { get; set; }
    }
}
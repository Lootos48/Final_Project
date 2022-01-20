using CardFile.Web.Models;
using System;
using System.Text;
using System.Web.Mvc;

namespace CardFile.Web.Helpers
{
    /// <summary>
    /// Класс для реализации методов пагинации
    /// </summary>
    public static class PagingHelpers
    {
        /// <summary>
        /// Метод для реализации пагинации
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pageInfo">Информация о требованиях к странице</param>
        /// <param name="pageUrl">Ссылка страницы</param>
        /// <returns>Строку элемента html</returns>
        public static MvcHtmlString PageLinks(this HtmlHelper html,
            PageInfoModel pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = pageInfo.PageNumber - 1; i <= pageInfo.PageNumber + 1; i++)
            {
                if (pageInfo.TotalPages <=0)
                {
                    break;
                }
                if (pageInfo.PageNumber < 1)
                {
                    i = 1;
                    pageInfo.PageNumber = 1;
                }
                else if (pageInfo.PageNumber > pageInfo.TotalPages)
                {
                    i = pageInfo.TotalPages - 1;
                    pageInfo.PageNumber = pageInfo.TotalPages;
                }
                TagBuilder tag = new TagBuilder("a");
                if (i == 0)
                {
                    i = 1;
                }
                string indexRef = $"{pageUrl(i)}&" +
                    $"FilterOptions={pageInfo.searchFilter.SearchBy}&" +
                    $"Value={pageInfo.searchFilter.SearchString}&" +
                    $"sortOrder={pageInfo.sortOptions}";
                tag.MergeAttribute("href", indexRef);
                tag.InnerHtml = i.ToString();
                if (i == pageInfo.PageNumber)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-dark");
                }
                tag.AddCssClass("btn btn-light");
                result.Append(tag.ToString());
                if (pageInfo.TotalPages == 1)
                {
                    break;
                }
                if (pageInfo.PageNumber + 1 > pageInfo.TotalPages)
                {
                    tag = new TagBuilder("a");
                    tag.MergeAttribute("href", indexRef);
                    tag.InnerHtml = pageInfo.TotalPages.ToString();
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-dark");
                    tag.AddCssClass("btn btn-light");
                    result.Append(tag.ToString());
                    break;
                }
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}
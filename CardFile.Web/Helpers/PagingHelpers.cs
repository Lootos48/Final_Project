using CardFile.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CardFile.Web.Helpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
        PageInfoModel pageInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();

            for (int i = pageInfo.PageNumber - 1; i <= pageInfo.PageNumber + 1; i++)
            {
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
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                // если текущая страница, то выделяем ее,
                // например, добавляя класс
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
                    tag.MergeAttribute("href", pageUrl(i));
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
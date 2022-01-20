using CardFile.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace CardFile.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionHandleFilter());
            filters.Add(new LoggerFilter());
        }
    }
}

using System;
using System.Web.Mvc;

namespace CardFile.Web.Filter
{
    public class ArgumentExceptionHandleFilterAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled && filterContext.Exception.GetType() == typeof(ArgumentException))
            {
                filterContext.Result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
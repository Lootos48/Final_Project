using CardFile.BLL.Infrastructure;
using System;
using System.Web.Mvc;

namespace CardFile.Web.Filter
{
    public class ArgumentExceptionHandleFilterAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            /*if (!filterContext.ExceptionHandled && filterContext.Exception.GetType() != typeof(ValidationException))
            {
                filterContext.Result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
                filterContext.ExceptionHandled = true;
            }*/
        }
    }
}
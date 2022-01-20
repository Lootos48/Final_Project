using CardFile.BLL.Infrastructure;
using Serilog;
using System;
using System.Web.Mvc;

namespace CardFile.Web.Filters
{
    /// <summary>
    /// Класс фильтра для обработки исключений во время выполнения программы
    /// </summary>
    public class ExceptionHandleFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                // логирование данных об исключении
                Log.Information("UnhandledException: \n\t" + filterContext.Exception + "\n\t Inner exception: " + filterContext.Exception.InnerException);

                filterContext.Result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
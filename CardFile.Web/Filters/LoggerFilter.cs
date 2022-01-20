using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CardFile.Web.Filters
{
    /// <summary>
    /// Класс фильтра реализации логгирования
    /// </summary>
    public class LoggerFilter : IActionFilter
    {
        /// <summary>
        /// Статический конструктор в котором задаётся способ логгирования, параматры логгирования и задаётся путь к файлу с логами
        /// </summary>
        static LoggerFilter()
        {
            string pathToFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\logs.log");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(pathToFile,
                fileSizeLimitBytes: 10000000,
                rollOnFileSizeLimit: true)
                .CreateLogger();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var logInfo = GetInfo(filterContext.RouteData.Values);
            Log.Logger.Information("Succesfully Executed: " + logInfo);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var logInfo = GetInfo(filterContext.RouteData.Values);
            Log.Logger.Information("Executing: " + logInfo);
        }

        /// <summary>
        /// Метод для составления строки 
        /// </summary>
        /// <param name="routeDictionary">Данные действия</param>
        /// <returns>Строку с информацией о произведённом действии</returns>
        private string GetInfo(RouteValueDictionary routeDictionary)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in routeDictionary)
            {
                stringBuilder.Append(item + "/");
            }

            return stringBuilder.ToString();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Infrastructure
{
    /// <summary>
    /// Класс для исключений вызванных использованием неправильных параметров
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// Свойство для передачи названия неверного поля
        /// </summary>
        public string Property { get; protected set; }

        /// <summary>
        /// Конструктор исключения
        /// </summary>
        /// <param name="message">Сообщение о причине исключения</param>
        /// <param name="prop">Название неверного поля</param>
        public ValidationException(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}

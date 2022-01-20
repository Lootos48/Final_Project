using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Infrastructure
{
    /// <summary>
    /// Исключение ошибки получения объекта
    /// </summary>
    /// <remarks>Исключение, которое вызываеться когда при выполнении операции получения объекта из БД по идентификатору - объект не был найден</remarks>

    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string message) : base(message)
        { }
    }
}

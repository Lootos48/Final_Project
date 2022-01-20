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
        /// <summary>
        /// Тип искомого объекта
        /// </summary>
        public Type ObjectType { get; set; }

        /// <summary>
        /// Идентификатор, по которому пытались получить объект
        /// </summary>
        public string ObjectId { get; set; }
        public ObjectNotFoundException(Type objectType, string objectId, string message) : base(message)
        {
            ObjectType = objectType;
            ObjectId = objectId;
        }
    }
}

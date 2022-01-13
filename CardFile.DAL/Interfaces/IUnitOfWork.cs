using CardFile.DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Interfaces
{
    /// <summary>
    /// Интерфейс для реализации паттерна Unit of Work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Поле доступа к репозиторию через которое будет производиться работа с БД с сущностями типа Author
        /// </summary>
        IRepository<Author> Authors { get; }

        /// <summary>
        /// Поле доступа к репозиторию через которое будет производиться работа с БД с сущностями типа Cards
        /// </summary>
        IRepository<Card> Cards { get; }
    }
}

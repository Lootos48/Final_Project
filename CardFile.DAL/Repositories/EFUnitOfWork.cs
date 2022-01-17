using CardFile.DAL.EF;
using CardFile.DAL.Entities;
using CardFile.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.Repositories
{
    /// <inheritdoc cref="IUnitOfWork"/>
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly CardFileContext db;

        /// <summary>
        /// Поле для сохранения объекта класса репозитория сущности Author
        /// </summary>
        private AuthorRepository authorRepository;

        /// <summary>
        /// Поле для сохранения объекта класса репозитория сущности Card
        /// </summary>
        private CardRepository cardRepository;

        /// <summary>
        /// Конструктор который создаёт новый объект контекста БД
        /// </summary>
        /// <param name="connectionString">Строка подключения к БД</param>
        /*public EFUnitOfWork(string connectionString)
        {
            db = new CardFileContext(connectionString);
        }*/
        public EFUnitOfWork(CardFileContext cardFileContext)
        {
            db = cardFileContext;
        }

        public IRepository<Author> Authors
        {
            // Если поле класса не инициализировано - инициализируем
            get
            {
                if (authorRepository == null)
                {
                    authorRepository = new AuthorRepository(db);
                }
                return authorRepository;
            }
        }

        public IRepository<Card> Cards
        {
            // Если поле класса не инициализировано - инициализируем
            get
            {
                if (cardRepository == null)
                {
                    cardRepository = new CardRepository(db);
                }
                return cardRepository;
            }
        }

         #region [ IDisposable interface implementation]
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
       
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

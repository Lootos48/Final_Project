using CardFile.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.DAL.EF
{
    /// <summary>
    /// Класс контекста базы данных
    /// </summary>
    public class CardFileContext : DbContext
    {
        public CardFileContext(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Поле коллекции сущностей  Author
        /// </summary>
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// Поле коллекции сущностей  Card
        /// </summary>
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // убираем добавление множественных окончаний у назвиний генерируемых таблиц
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // создаём связь 1-к-многим у таблиц Author и Cards 
            modelBuilder.Entity<Author>()
                .HasMany(c => c.Cards)
                .WithRequired(a => a.Author)
                .HasForeignKey(k => k.AuthorId)
                .WillCascadeOnDelete(false);
        }

        /// <summary>
        /// Метод для вызова процедуры добавления значения ID Author в колонку AuthorId строки таблицы Card
        /// </summary>
        /// <param name="cardId">ID карточки с которую добавляем значение ID автора</param>
        /// <param name="authorId">ID автора что создал карточку</param>
        /// <returns></returns>
        public virtual Task<int> AddAuthorToCard(int cardId, int authorId)
        {
            return this.Database.ExecuteSqlCommandAsync("exec AddAuthorToCard @cardId, @authorId",
                new SqlParameter("@cardId", cardId),
                new SqlParameter("@authorId", authorId));
        }
    }


    /// <summary>
    /// Класс для обеспечения работы Code-first миграций, который позволяет миграциям создавать объект класса контекста БД
    /// </summary>
    public class CardFileContextFactory : IDbContextFactory<CardFileContext>
    {
        public CardFileContext Create()
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=CardFileDB;Trusted_Connection=True;";
            return new CardFileContext(connectionString);
        }
    }
}

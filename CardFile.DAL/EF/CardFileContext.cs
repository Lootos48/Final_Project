using CardFile.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
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
                .WithRequired(a => a.Author);
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

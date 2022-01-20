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

        /// <summary>
        /// Поле коллекции сущностей  Author
        /// </summary>
        public DbSet<Author> Authors { get; set; }

        /// <summary>
        /// Поле коллекции сущностей  Card
        /// </summary>
        public DbSet<Card> Cards { get; set; }

        /// <summary>
        /// Поле для хранения информации о пролайканных автором карточках (связь М-к-М)
        /// </summary>
        public DbSet<AuthorsLikedCards> AuthorsLikedCards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // убираем добавление множественных окончаний у назвиний генерируемых таблиц
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // создаём связь 1-к-многим у таблиц Author и Cards  и отключаем каскадное удаление
            modelBuilder.Entity<Author>()
                .HasMany(c => c.Cards)
                .WithRequired(a => a.Author)
                .HasForeignKey(k => k.AuthorId)
                .WillCascadeOnDelete(false);

            // явно задали композитный ключ
            modelBuilder.Entity<AuthorsLikedCards>()
                .HasKey(x => new { x.AuthorId, x.CardId });
        }
    }
}

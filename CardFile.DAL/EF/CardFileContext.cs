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
    public class CardFileContext : DbContext
    {
        public CardFileContext(string connectionString) : base(connectionString) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Author>()
                .HasMany(c => c.Cards)
                .WithRequired(a => a.Author);
        }
    }

    public class CardFileContextFactory : IDbContextFactory<CardFileContext>
    {
        public CardFileContext Create()
        {
            string connectionString = @"Server=(localdb)\mssqllocaldb;Database=CardFileDB;Trusted_Connection=True;";
            return new CardFileContext(connectionString);
        }
    }
}

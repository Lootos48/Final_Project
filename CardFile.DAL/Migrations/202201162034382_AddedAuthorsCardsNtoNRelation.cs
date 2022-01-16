namespace CardFile.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAuthorsCardsNtoNRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthorsLikedCards",
                c => new
                    {
                        AuthorId = c.Int(nullable: false),
                        CardId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AuthorId, t.CardId })
                .ForeignKey("dbo.Author", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Card", t => t.CardId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.CardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthorsLikedCards", "CardId", "dbo.Card");
            DropForeignKey("dbo.AuthorsLikedCards", "AuthorId", "dbo.Author");
            DropIndex("dbo.AuthorsLikedCards", new[] { "CardId" });
            DropIndex("dbo.AuthorsLikedCards", new[] { "AuthorId" });
            DropTable("dbo.AuthorsLikedCards");
        }
    }
}

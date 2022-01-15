namespace CardFile.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUpdateProcedure : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("AddAuthorToCard", p => new { cardId = p.Int(), authorId = p.Int() }, @"
                UPDATE dbo.Card
                SET dbo.Card.Author_Id = @authorId
                WHERE dbo.Card.Id = @cardId
            ");
        }
        
        public override void Down()
        {
            DropStoredProcedure("AddAuthorToCard");
        }
    }
}

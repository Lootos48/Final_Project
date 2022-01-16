namespace CardFile.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReCreateOf1toManyRelationOfAuthorAndCards : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Card", "Author_Id", "dbo.Author");
            RenameColumn(table: "dbo.Card", name: "Author_Id", newName: "AuthorId");
            RenameIndex(table: "dbo.Card", name: "IX_Author_Id", newName: "IX_AuthorId");
            AddForeignKey("dbo.Card", "AuthorId", "dbo.Author", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Card", "AuthorId", "dbo.Author");
            RenameIndex(table: "dbo.Card", name: "IX_AuthorId", newName: "IX_Author_Id");
            RenameColumn(table: "dbo.Card", name: "AuthorId", newName: "Author_Id");
            AddForeignKey("dbo.Card", "Author_Id", "dbo.Author", "Id", cascadeDelete: true);
        }
    }
}

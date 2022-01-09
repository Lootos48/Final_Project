namespace CardFile.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StringConstraintsWasChanged : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Card", "Text", c => c.String(nullable: false, maxLength: 2000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Card", "Text", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}

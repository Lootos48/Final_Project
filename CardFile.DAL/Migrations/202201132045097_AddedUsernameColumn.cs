namespace CardFile.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUsernameColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Author", "Username", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Author", "Username");
        }
    }
}

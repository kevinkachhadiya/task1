namespace task1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m003 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Logins");
            AddColumn("dbo.Logins", "Email", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Logins", "ConfirmPassword", c => c.String(nullable: false));
            AddPrimaryKey("dbo.Logins", "Email");
            DropColumn("dbo.Logins", "Id");
            DropColumn("dbo.Logins", "UserName");
            DropColumn("dbo.Logins", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logins", "Password", c => c.String());
            AddColumn("dbo.Logins", "UserName", c => c.String());
            AddColumn("dbo.Logins", "Id", c => c.Int(nullable: false, identity: true));
            DropPrimaryKey("dbo.Logins");
            DropColumn("dbo.Logins", "ConfirmPassword");
            DropColumn("dbo.Logins", "Email");
            AddPrimaryKey("dbo.Logins", "Id");
        }
    }
}

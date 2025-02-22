namespace task1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Countries", "UserData_user_id", "dbo.UserDatas");
            DropForeignKey("dbo.States", "UserData_user_id", "dbo.UserDatas");
            DropIndex("dbo.States", new[] { "UserData_user_id" });
            DropIndex("dbo.Countries", new[] { "UserData_user_id" });
            DropColumn("dbo.States", "UserData_user_id");
            DropColumn("dbo.Countries", "UserData_user_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Countries", "UserData_user_id", c => c.Int());
            AddColumn("dbo.States", "UserData_user_id", c => c.Int());
            CreateIndex("dbo.Countries", "UserData_user_id");
            CreateIndex("dbo.States", "UserData_user_id");
            AddForeignKey("dbo.States", "UserData_user_id", "dbo.UserDatas", "user_id");
            AddForeignKey("dbo.Countries", "UserData_user_id", "dbo.UserDatas", "user_id");
        }
    }
}

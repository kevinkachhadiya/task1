namespace task1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.States", "UserData_user_id", c => c.Int());
            AddColumn("dbo.UserDatas", "SelectedStateId", c => c.Int(nullable: false));
            CreateIndex("dbo.States", "UserData_user_id");
            AddForeignKey("dbo.States", "UserData_user_id", "dbo.UserDatas", "user_id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.States", "UserData_user_id", "dbo.UserDatas");
            DropIndex("dbo.States", new[] { "UserData_user_id" });
            DropColumn("dbo.UserDatas", "SelectedStateId");
            DropColumn("dbo.States", "UserData_user_id");
        }
    }
}

using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class m2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDatas", "country_country_id", "dbo.Countries");
            DropIndex("dbo.UserDatas", new[] { "country_country_id" });
            AddColumn("dbo.Countries", "UserData_user_id", c => c.Int());
            AlterColumn("dbo.UserDatas", "Dob", c => c.String(nullable: false));
            CreateIndex("dbo.Countries", "UserData_user_id");
            AddForeignKey("dbo.Countries", "UserData_user_id", "dbo.UserDatas", "user_id");
            DropColumn("dbo.UserDatas", "country_country_id");
        }

        public override void Down()
        {
            AddColumn("dbo.UserDatas", "country_country_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Countries", "UserData_user_id", "dbo.UserDatas");
            DropIndex("dbo.Countries", new[] { "UserData_user_id" });
            AlterColumn("dbo.UserDatas", "Dob", c => c.DateTime(nullable: false));
            DropColumn("dbo.Countries", "UserData_user_id");
            CreateIndex("dbo.UserDatas", "country_country_id");
            AddForeignKey("dbo.UserDatas", "country_country_id", "dbo.Countries", "country_id", cascadeDelete: true);
        }
    }
}

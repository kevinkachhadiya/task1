using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class _20 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserDatas", "ConfirmPassword");
        }

        public override void Down()
        {
            AddColumn("dbo.UserDatas", "ConfirmPassword", c => c.String(nullable: false));
        }
    }
}

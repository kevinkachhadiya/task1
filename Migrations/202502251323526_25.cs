using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class _25 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDatas", "Gender1", c => c.Int(nullable: false));
            DropColumn("dbo.UserDatas", "Gender");
        }

        public override void Down()
        {
            AddColumn("dbo.UserDatas", "Gender", c => c.Int(nullable: false));
            DropColumn("dbo.UserDatas", "Gender1");
        }
    }
}

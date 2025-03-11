using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class _30 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDatas", "IsActive", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.UserDatas", "IsActive");
        }
    }
}

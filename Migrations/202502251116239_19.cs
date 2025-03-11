using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class _19 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserDatas", "SelectedCountryId");
            DropColumn("dbo.UserDatas", "SelectedStateId");
        }

        public override void Down()
        {
            AddColumn("dbo.UserDatas", "SelectedStateId", c => c.Int(nullable: false));
            AddColumn("dbo.UserDatas", "SelectedCountryId", c => c.Int(nullable: false));
        }
    }
}

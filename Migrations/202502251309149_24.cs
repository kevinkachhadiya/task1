using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class _24 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserDatas", "GenderString");
        }

        public override void Down()
        {
            AddColumn("dbo.UserDatas", "GenderString", c => c.String());
        }
    }
}

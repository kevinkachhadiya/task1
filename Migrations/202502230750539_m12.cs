using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class m12 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserDatas", "ImagePath", c => c.String());
        }

        public override void Down()
        {
            AlterColumn("dbo.UserDatas", "ImagePath", c => c.String(nullable: false));
        }
    }
}

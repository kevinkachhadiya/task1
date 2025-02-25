namespace task1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _26 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDatas", "GenderString", c => c.String());
            DropColumn("dbo.UserDatas", "Gender1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserDatas", "Gender1", c => c.Int(nullable: false));
            DropColumn("dbo.UserDatas", "GenderString");
        }
    }
}

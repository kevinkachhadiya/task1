namespace task1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _27 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDatas", "Gender_", c => c.String());
            DropColumn("dbo.UserDatas", "GenderString");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserDatas", "GenderString", c => c.String());
            DropColumn("dbo.UserDatas", "Gender_");
        }
    }
}

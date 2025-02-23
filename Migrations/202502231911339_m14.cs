namespace task1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m14 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserDatas", "SelectedCountryId", c => c.Int());
            AlterColumn("dbo.UserDatas", "SelectedStateId", c => c.Int());
            AlterColumn("dbo.UserDatas", "SelectedCityId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserDatas", "SelectedCityId", c => c.Int(nullable: false));
            AlterColumn("dbo.UserDatas", "SelectedStateId", c => c.Int(nullable: false));
            AlterColumn("dbo.UserDatas", "SelectedCountryId", c => c.Int(nullable: false));
        }
    }
}

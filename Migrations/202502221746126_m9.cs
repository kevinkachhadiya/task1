namespace task1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m9 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Cities", "city_idd");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cities", "city_idd", c => c.Int(nullable: false));
        }
    }
}

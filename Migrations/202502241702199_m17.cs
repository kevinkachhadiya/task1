using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class m17 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDatas", "selectedCity_city_id", "dbo.Cities");
            DropForeignKey("dbo.UserDatas", "SelectedCountry_country_id", "dbo.Countries");
            DropForeignKey("dbo.UserDatas", "selectedState_state_id", "dbo.States");
            DropIndex("dbo.UserDatas", new[] { "selectedCity_city_id" });
            DropIndex("dbo.UserDatas", new[] { "SelectedCountry_country_id" });
            DropIndex("dbo.UserDatas", new[] { "selectedState_state_id" });
            DropColumn("dbo.UserDatas", "selectedCity_city_id");
            DropColumn("dbo.UserDatas", "SelectedCountry_country_id");
            DropColumn("dbo.UserDatas", "selectedState_state_id");
        }

        public override void Down()
        {
            AddColumn("dbo.UserDatas", "selectedState_state_id", c => c.Int());
            AddColumn("dbo.UserDatas", "SelectedCountry_country_id", c => c.Int());
            AddColumn("dbo.UserDatas", "selectedCity_city_id", c => c.Int());
            CreateIndex("dbo.UserDatas", "selectedState_state_id");
            CreateIndex("dbo.UserDatas", "SelectedCountry_country_id");
            CreateIndex("dbo.UserDatas", "selectedCity_city_id");
            AddForeignKey("dbo.UserDatas", "selectedState_state_id", "dbo.States", "state_id");
            AddForeignKey("dbo.UserDatas", "SelectedCountry_country_id", "dbo.Countries", "country_id");
            AddForeignKey("dbo.UserDatas", "selectedCity_city_id", "dbo.Cities", "city_id");
        }
    }
}

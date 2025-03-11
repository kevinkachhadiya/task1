using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                {
                    city_id = c.Int(nullable: false, identity: true),
                    CityName = c.String(nullable: false),
                    state_id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.city_id)
                .ForeignKey("dbo.States", t => t.state_id, cascadeDelete: true)
                .Index(t => t.state_id);

            CreateTable(
                "dbo.States",
                c => new
                {
                    state_id = c.Int(nullable: false, identity: true),
                    StateName = c.String(nullable: false),
                    country_id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.state_id)
                .ForeignKey("dbo.Countries", t => t.country_id, cascadeDelete: true)
                .Index(t => t.country_id);

            CreateTable(
                "dbo.Countries",
                c => new
                {
                    country_id = c.Int(nullable: false, identity: true),
                    CountryName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.country_id);

            CreateTable(
                "dbo.UserDatas",
                c => new
                {
                    user_id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false),
                    LastName = c.String(nullable: false),
                    Email = c.String(nullable: false),
                    MobileNo = c.String(nullable: false),
                    Gender = c.String(nullable: false),
                    Dob = c.DateTime(nullable: false),
                    Password = c.String(nullable: false),
                    ConfirmPassword = c.String(nullable: false),
                    Address = c.String(nullable: false),
                    ImagePath = c.String(nullable: false),
                    country_country_id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.user_id)
                .ForeignKey("dbo.Countries", t => t.country_country_id, cascadeDelete: true)
                .Index(t => t.country_country_id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.UserDatas", "country_country_id", "dbo.Countries");
            DropForeignKey("dbo.States", "country_id", "dbo.Countries");
            DropForeignKey("dbo.Cities", "state_id", "dbo.States");
            DropIndex("dbo.UserDatas", new[] { "country_country_id" });
            DropIndex("dbo.States", new[] { "country_id" });
            DropIndex("dbo.Cities", new[] { "state_id" });
            DropTable("dbo.UserDatas");
            DropTable("dbo.Countries");
            DropTable("dbo.States");
            DropTable("dbo.Cities");
        }
    }
}

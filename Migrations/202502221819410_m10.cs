﻿using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class m10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDatas", "SelectedCityId", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.UserDatas", "SelectedCityId");
        }
    }
}

﻿using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class m4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDatas", "SelectedCountryId", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.UserDatas", "SelectedCountryId");
        }
    }
}

﻿using System;
using System.Data.Entity.Migrations;

namespace task1.Migrations
{
    public partial class m8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cities", "city_idd", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Cities", "city_idd");
        }
    }
}

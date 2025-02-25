﻿namespace task1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDatas", "GenderString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserDatas", "GenderString");
        }
    }
}

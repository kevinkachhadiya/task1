using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace task1.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<task1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(task1.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}

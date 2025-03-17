using System.Data.Entity;
namespace task1.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection") { }

        public DbSet<UserData> Users { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<State> states { get; set; }

        public DbSet<City> Cities { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}

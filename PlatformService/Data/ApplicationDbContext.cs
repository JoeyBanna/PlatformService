using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
         public DbSet<Models.Platform> Platforms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Platform>().HasData(
                new Models.Platform()
                {
                    Id = 1,
                    Name = "SQL Server Express",
                    Publisher = "Microsoft",
                    Cost = "Free"
                    
                }
                );
        }
    }
}

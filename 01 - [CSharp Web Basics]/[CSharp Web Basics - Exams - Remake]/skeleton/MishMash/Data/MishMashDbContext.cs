using Microsoft.EntityFrameworkCore;

namespace MishMashWebApp.Data
{
    public class MishMashDbContext : DbContext
    {
        public MishMashDbContext()
        {
        }

        public MishMashDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

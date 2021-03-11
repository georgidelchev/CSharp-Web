using Microsoft.EntityFrameworkCore;

namespace Andreys.Data
{
    public class AndreysDbContext : DbContext
    {
        public AndreysDbContext()
        {
        }

        public AndreysDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer($"Server=.;Database=Andreys;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

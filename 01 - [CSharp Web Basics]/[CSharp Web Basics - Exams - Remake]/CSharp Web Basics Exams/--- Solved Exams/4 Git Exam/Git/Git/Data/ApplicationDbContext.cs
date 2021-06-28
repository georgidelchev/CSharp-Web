using Microsoft.EntityFrameworkCore;

namespace Git.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<Commit> Commits { get; set; }

        public DbSet<Repository> Repositories { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=Git;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Repository>(repository =>
            {
                repository
                    .HasOne(r => r.Owner)
                    .WithMany(o => o.Repositories)
                    .HasForeignKey(r => r.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Commit>(commit =>
            {
                commit
                    .HasOne(c => c.Creator)
                    .WithMany(o => o.Commits)
                    .HasForeignKey(c => c.CreatorId)
                    .OnDelete(DeleteBehavior.Restrict);

                commit
                    .HasOne(c => c.Repository)
                    .WithMany(r => r.Commits)
                    .HasForeignKey(c => c.RepositoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
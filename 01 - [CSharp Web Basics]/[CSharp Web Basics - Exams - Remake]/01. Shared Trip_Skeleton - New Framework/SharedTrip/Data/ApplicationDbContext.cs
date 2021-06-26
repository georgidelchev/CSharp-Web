using Microsoft.EntityFrameworkCore;

namespace SharedTrip.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Trip> Trips { get; set; }

        public DbSet<UserTrip> UserTrips { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTrip>(userTrip =>
            {
                userTrip
                    .HasKey(ut => new
                    {
                        ut.UserId,
                        ut.TripId
                    });
            });
        }
    }
}

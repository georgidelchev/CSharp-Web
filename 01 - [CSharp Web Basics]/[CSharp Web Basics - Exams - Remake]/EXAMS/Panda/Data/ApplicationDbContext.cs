using Microsoft.EntityFrameworkCore;

namespace Panda.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Package> Packages { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=DESKTOP-10E0DVG\\SQLEXPRESS01;Database=Panda;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>(package =>
            {
                package
                    .HasOne(p => p.Recipient)
                    .WithMany(r => r.Packages)
                    .HasForeignKey(p => p.RecipientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Receipt>(receipt =>
            {
                receipt
                    .HasOne(r => r.Recipient)
                    .WithMany(re => re.Receipts)
                    .HasForeignKey(r => r.RecipientId)
                    .OnDelete(DeleteBehavior.Restrict);

                receipt
                    .HasOne(r => r.Package);
            });
        }

    }
}
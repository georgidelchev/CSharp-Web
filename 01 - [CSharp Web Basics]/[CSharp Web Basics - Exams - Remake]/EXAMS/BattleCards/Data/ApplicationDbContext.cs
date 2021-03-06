﻿using Microsoft.EntityFrameworkCore;

namespace BattleCards.Data
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

        public DbSet<Card> Cards { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserCard> UserCards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(
                        $"Server=DESKTOP-10E0DVG\\SQLEXPRESS01;Database=BattleCards;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserCard>(userCard =>
                {
                    userCard
                        .HasKey(uc => new
                        {
                            uc.CardId,
                            uc.UserId
                        });
                });
        }
    }
}

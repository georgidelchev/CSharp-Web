﻿using Microsoft.EntityFrameworkCore;

namespace BattleCards.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(
                        $"Server=DESKTOP-10E0DVG\\SQLEXPRESS01;Database=BattleCards;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}

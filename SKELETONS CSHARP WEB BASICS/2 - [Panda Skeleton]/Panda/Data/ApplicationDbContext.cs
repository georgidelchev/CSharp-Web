﻿using Microsoft.EntityFrameworkCore;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=DESKTOP-10E0DVG\\SQLEXPRESS01;Database=Panda;Integrated Security=True;");
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;

namespace MishMash.Data
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

        public DbSet<Channel> Channels { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserChannel> UserChannels { get; set; }

        public DbSet<ChannelTag> ChannelTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=.;Database=MishMash;Integrated Security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelTag>(channelTag =>
            {
                channelTag
                    .HasOne(ct => ct.Channel)
                    .WithMany(c => c.Tags)
                    .HasForeignKey(ct => ct.ChannelId)
                    .OnDelete(DeleteBehavior.Restrict);

                channelTag
                    .HasOne(ct => ct.Tag)
                    .WithMany(c => c.Channels)
                    .HasForeignKey(ct => ct.TagId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}

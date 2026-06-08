using GameStore.Models.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<LibraryEntry> LibraryEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Game>()
                .HasOne(g => g.Genre)
                .WithMany(ge => ge.Games)
                .HasForeignKey(g => g.GenreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Game>()
                .HasOne(g => g.Publisher)
                .WithMany(p => p.Games)
                .HasForeignKey(g => g.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Game)
                .WithMany(g => g.Reviews)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LibraryEntry>()
                .HasOne(l => l.Game)
                .WithMany(g => g.LibraryEntries)
                .HasForeignKey(l => l.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<LibraryEntry>()
                .HasIndex(l => new { l.UserId, l.GameId })
                .IsUnique();
        }
    }
}

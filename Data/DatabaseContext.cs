using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TableTennisAPI.Models;

namespace TableTennisAPI.Data {
    public class DatabaseContext : DbContext {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .HasOne(e => e.MatchWinner)
                .WithMany(u => u.MatchAsWinner)
                .HasForeignKey(m => m.MatchWinnerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.MatchLoser)
                .WithMany(u => u.MatchAsLoser)
                .HasForeignKey(m => m.MatchLoserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}

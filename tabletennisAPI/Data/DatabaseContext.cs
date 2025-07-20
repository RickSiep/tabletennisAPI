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
                .HasIndex(e => e.Id)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Matches)
                .WithMany(e => e.Users);

            modelBuilder.Entity<UserMatch>()
                .HasKey(um => new { um.UserId, um.MatchId});

            modelBuilder.Entity<UserMatch>()
                .HasOne<User>()
                .WithMany(u => u.UserMatches)
                .HasForeignKey(um  => um.UserId);

            modelBuilder.Entity<UserMatch>()
                .HasOne<Match>()
                .WithMany(m => m.UserMatches)
                .HasForeignKey(um =>um.MatchId);
        }
    }
}

using Roulette.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Configuration;

namespace Roulette.DataAccess
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<PlayerDetail> PlayerDetails { get; set; }
        public DbSet<GameTransaction> GameTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("FileName=RouletteDb.db", option =>
            {
                option.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerDetail>().ToTable("PlayerDetails");
            modelBuilder.Entity<PlayerDetail>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.PlayerName).IsUnique();
            });

            modelBuilder.Entity<GameTransaction>().ToTable("GameTransactions");
            modelBuilder.Entity<GameTransaction>(entity =>
            {
                entity.HasKey(x => x.Id);

            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

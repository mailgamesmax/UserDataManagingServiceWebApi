using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using UserDataManagingService.Models;

namespace UserDataManagingService
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<LivingPlace> LivingPlaces { get; set; }
        public DbSet<Avatar> Avatars { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.NickName)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .Property(e => e.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (Role)Enum.Parse(typeof(Role), v));

            // user-living place 1to1
            modelBuilder
                .Entity<User>()
                .HasOne(u => u.LivingPlace)
                .WithOne(c => c.User)
                .HasForeignKey<LivingPlace>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // user-avatar 1to1
            modelBuilder
                .Entity<User>()
                .HasOne(u => u.Avatar)
                .WithOne(c => c.User)
                .HasForeignKey<Avatar>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}

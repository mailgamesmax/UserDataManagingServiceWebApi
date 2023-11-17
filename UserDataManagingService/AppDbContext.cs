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
            modelBuilder
                .Entity<User>()
                .HasOne(u => u.LivingPlace)
                .WithOne(c => c.User)
                .HasForeignKey<LivingPlace>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

/*
            // catTitle-catContent 1toM
            modelBuilder
                .Entity<NoteCategory>()
                .HasMany(u => u.NoteContents)
                .WithOne(n => n.NoteCategory)
                .HasForeignKey(k => k.Cat_Id);
*/
        }

    }
}

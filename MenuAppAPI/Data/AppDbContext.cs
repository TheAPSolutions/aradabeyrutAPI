using AradaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AradaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Videos> Videos { get; set; }
        public DbSet<Coupons> Coupons { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Video) // MenuItem has one Video
                .WithOne(v => v.menuItem) // Video has one MenuItem
                .HasForeignKey<Videos>(v => v.videoID); // Explicitly specify the foreign key
        }
    }
}



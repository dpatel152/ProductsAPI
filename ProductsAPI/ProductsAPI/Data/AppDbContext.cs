using ProductsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Buyer> Buyers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU)
                .IsUnique(); // Ensure SKU is unique

            modelBuilder.Entity<Buyer>().HasData(
                new Buyer { Id = "49ec2a8703224eea9dec16b22546477e", Name = "Johnny Buyer", Email = "jbuyer@test.com" },
                new Buyer { Id = "a790a7b6bf2a48569066c46306c3332d", Name = "Jennie Purchaser", Email = "jpurchaser@test.com" }
            );
        }
    }
}

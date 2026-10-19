
using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data;

public class ProductDbContext : DbContext
{
    public ProductDbContext(
        DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        // Initial products for our development database.
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Wireless Mouse",
                Description = "Wireless mouse with USB receiver",
                Price = 799.00m,
                StockQuantity = 50
            },
            new Product
            {
                Id = 2,
                Name = "Mechanical Keyboard",
                Description = "Mechanical keyboard with RGB lighting",
                Price = 2499.00m,
                StockQuantity = 25
            },
            new Product
            {
                Id = 3,
                Name = "USB-C Hub",
                Description = "USB-C hub with multiple ports",
                Price = 1299.00m,
                StockQuantity = 30
            }
        );
    }
}

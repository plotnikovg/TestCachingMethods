using Microsoft.EntityFrameworkCore;
using InMemoryCachingDomain.Entities;

namespace InMemoryCachingInfrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}
    
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Cart> Carts => Set<Cart>();

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     
    // }

    public override int SaveChanges()
    {
        return SaveChangesAsync().GetAwaiter().GetResult();
    }
}
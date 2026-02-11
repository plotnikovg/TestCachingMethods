using WithoutCachingDomain.Entities;

namespace WithoutCachingInfrastructure;

public class AppDbContextInitializer
{
    private readonly AppDbContext _context;

    public AppDbContextInitializer(AppDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task SeedAsync()
    {
        if (!_context.Categories.Any())
        {
            for (int i = 0; i < 60; i++)
            {
                _context.Categories.Add(new Category
                {
                    Name = "Category " + (i + 1)
                });
            }
            
            await _context.SaveChangesAsync();
        }
        
        if (!_context.Products.Any())
        {
            var rand = new Random();
            for (int i = 0; i < 60000; i++)
            {
                _context.Products.Add(new Product
                {
                    Name = "Product " + (i + 1),
                    Price = new decimal(Math.Round(rand.NextDouble() * 6600, 2)),
                    CategoryId = rand.Next(1, 61),
                    Description = "Description of product " + (i + 1),
                    IsInStock = true
                });
            }
            
            await _context.SaveChangesAsync();
        }
    }
}
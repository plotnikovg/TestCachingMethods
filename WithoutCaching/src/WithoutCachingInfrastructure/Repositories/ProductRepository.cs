using Microsoft.EntityFrameworkCore;
using WithoutCachingDomain.Entities;
using WithoutCachingDomain.Interfaces;

namespace WithoutCachingInfrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProductsRangeByIdAsync(int firstId, int lastId)
    {
        return await _context.Products
            .Where(p => p.Id >= firstId && p.Id <= lastId)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
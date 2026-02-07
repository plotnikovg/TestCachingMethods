using Microsoft.EntityFrameworkCore;
using InMemoryCachingDomain.Entities;
using InMemoryCachingDomain.Interfaces;

namespace InMemoryCachingInfrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProductsRangeByIdAsync(int? categoryId, int firstId, int lastId)
    {
        if (categoryId == null)
        {
            return await _context.Products
                .Where(p => p.Id >= firstId && p.Id <= lastId)
                .ToListAsync();
        }

        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .OrderBy(p => p.Id)
            .Skip(firstId - 1)
            .Take(lastId - firstId + 1)
            .ToListAsync();

    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
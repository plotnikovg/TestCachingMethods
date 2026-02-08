using Microsoft.EntityFrameworkCore;
using DistributedCachingRedisDomain.Entities;
using DistributedCachingRedisDomain.Interfaces;

namespace DistributedCachingRedisInfrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }
}
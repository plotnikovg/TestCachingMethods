using System.Text.Json;
using DistributedCachingRedisDomain.Entities;
using DistributedCachingRedisDomain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCachingRedisInfrastructure.Repositories.Cached;

public class CachedCategoryRepository : ICategoryRepository
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDistributedCache _cache;

    public CachedCategoryRepository(ICategoryRepository categoryRepository, IDistributedCache cache)
    {
        _categoryRepository = categoryRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var categoriesBytes = await _cache.GetAsync($"Categories");

        if (categoriesBytes != null)
        {
            return JsonSerializer.Deserialize<List<Category>>(categoriesBytes);
        }

        var categories = (await _categoryRepository.GetAllAsync()).ToList();
        
        categoriesBytes = JsonSerializer.SerializeToUtf8Bytes(categories);
        await _cache.SetAsync($"Categories", categoriesBytes);
        
        return categories;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }
}
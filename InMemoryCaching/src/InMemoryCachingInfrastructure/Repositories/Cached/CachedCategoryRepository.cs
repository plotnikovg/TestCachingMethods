using InMemoryCachingDomain.Entities;
using InMemoryCachingDomain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCachingInfrastructure.Repositories.Cached;

public class CachedCategoryRepository : ICategoryRepository
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMemoryCache _cache;

    public CachedCategoryRepository(ICategoryRepository categoryRepository, IMemoryCache cache)
    {
        _categoryRepository = categoryRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        if (!_cache.TryGetValue($"AllCategories", out IEnumerable<Category> categories))
        {
            categories = await _categoryRepository.GetAllAsync();
            
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(categories.Count())
                .SetPriority(CacheItemPriority.NeverRemove);
            
            _cache.Set($"AllCategories", categories, cacheEntryOptions);
        }
        
        return categories;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        if (!_cache.TryGetValue($"Category_{id}", out Category? category))
        {
            category = await _categoryRepository.GetByIdAsync(id);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1);
            
            _cache.Set($"Category_{id}", category, cacheEntryOptions);
        }
        
        return category;
    }
}
using HybridCachingDomain.Entities;
using HybridCachingDomain.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;

namespace HybridCachingInfrastructure.Repositories.Cached;

public class CachedCategoryRepository : ICategoryRepository
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly HybridCache _cache;

    public CachedCategoryRepository(ICategoryRepository categoryRepository, HybridCache cache)
    {
        _categoryRepository = categoryRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _cache.GetOrCreateAsync(
            $"Categories",
            async req => await _categoryRepository.GetAllAsync());
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _cache.GetOrCreateAsync(
            $"Category_{id}",
            async req => await _categoryRepository.GetByIdAsync(id));
    }
}
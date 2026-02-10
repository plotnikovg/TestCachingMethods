using HybridCachingDomain.Entities;
using HybridCachingDomain.Interfaces;
using Microsoft.Extensions.Caching.Hybrid;

namespace HybridCachingInfrastructure.Repositories.Cached;

public class CachedProductRepository : IProductRepository
{
    private readonly IProductRepository _productRepository;
    private readonly HybridCache _cache;

    public CachedProductRepository(IProductRepository productRepository, HybridCache cache)
    {
        _productRepository = productRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<Product>> GetProductsRangeByIdAsync(int? categoryId, int firstId, int lastId)
    {
        var cacheOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromSeconds(66),
            LocalCacheExpiration = TimeSpan.FromSeconds(6)
        };
        
        return await _cache.GetOrCreateAsync(
            $"products_{categoryId}_{firstId}_{lastId}",
            async req => await _productRepository
                .GetProductsRangeByIdAsync(categoryId, firstId, lastId),
            cacheOptions);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await  _cache.GetOrCreateAsync(
            $"product_{id}",
            async req => await _productRepository
                .GetProductByIdAsync(id));
    }
}
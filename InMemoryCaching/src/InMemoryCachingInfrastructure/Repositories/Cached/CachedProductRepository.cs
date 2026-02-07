using InMemoryCachingDomain.Entities;
using InMemoryCachingDomain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCachingInfrastructure.Repositories.Cached;

public class CachedProductRepository : IProductRepository
{
    private readonly IProductRepository _productRepository;
    private readonly IMemoryCache _cache;

    public CachedProductRepository(IProductRepository productRepository, IMemoryCache cache)
    {
        _productRepository = productRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<Product>> GetProductsRangeByIdAsync(int? categoryId, int firstId, int lastId)
    {
        if (!_cache.TryGetValue($"ProductsList_{categoryId}_{firstId}_{lastId}", out IEnumerable<Product> products))
        {
            products = await _productRepository.GetProductsRangeByIdAsync(categoryId, firstId, lastId);
            
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(products.Count())
                .SetPriority(CacheItemPriority.Low);
            
            _cache.Set($"ProductsList_{categoryId}_{firstId}_{lastId}", products, cacheEntryOptions);
        }
        
        return products;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        if (!_cache.TryGetValue($"Product_{id}", out Product? product))
        {
            product = await _productRepository.GetProductByIdAsync(id);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1);
            
            _cache.Set($"Product_{id}", product, cacheEntryOptions);
        }
        
        return product;
    }
}
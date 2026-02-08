using System.Text.Json;
using DistributedCachingRedisDomain.Entities;
using DistributedCachingRedisDomain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCachingRedisInfrastructure.Repositories.Cached;

public class CachedProductRepository : IProductRepository
{
    private readonly IProductRepository _productRepository;
    private readonly IDistributedCache _cache;

    public CachedProductRepository(IProductRepository productRepository, IDistributedCache cache)
    {
        _productRepository = productRepository;
        _cache = cache;
    }

    public async Task<IEnumerable<Product>> GetProductsRangeByIdAsync(int? categoryId, int firstId, int lastId)
    {
        var productsBytes = await _cache.GetAsync($"Products_{categoryId}_{firstId}_{lastId}");
        if (productsBytes != null)
        {
            return JsonSerializer.Deserialize<List<Product>>(productsBytes);
        }
        else
        {
            var products = (await _productRepository.GetProductsRangeByIdAsync(categoryId, firstId, lastId)).ToList();
            
            productsBytes = JsonSerializer.SerializeToUtf8Bytes(products);
            await _cache.SetAsync($"Products_{categoryId}_{firstId}_{lastId}", productsBytes);
            
            return products;
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var productBytes = await _cache.GetAsync($"Product_{id}");

        if (productBytes != null)
        {
            return JsonSerializer.Deserialize<Product>(productBytes);
        }

        var product = await _productRepository.GetProductByIdAsync(id);

        productBytes = JsonSerializer.SerializeToUtf8Bytes(product);
        await _cache.SetAsync($"Product_{id}", productBytes);
        
        return product;
    }
}
using WithoutCachingDomain.Entities;

namespace WithoutCachingDomain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsRangeByIdAsync(int firstId, int lastId);
    Task<Product?> GetProductByIdAsync(int id);
}
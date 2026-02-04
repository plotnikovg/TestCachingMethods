using WithoutCachingDomain.Entities;

namespace WithoutCachingDomain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsRangeById(int firstId, int lastId);
    Task<Product> GetProductById(int id);
}
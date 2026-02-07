using InMemoryCachingDomain.Entities;

namespace InMemoryCachingDomain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsRangeByIdAsync(int? categoryId, int firstId, int lastId);
    Task<Product?> GetProductByIdAsync(int id);
}
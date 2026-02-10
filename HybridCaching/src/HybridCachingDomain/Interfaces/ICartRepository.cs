using HybridCachingDomain.Entities;

namespace HybridCachingDomain.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByIdAsync(int id);
    void AddProduct(Cart cart);
    void Update(Cart cart);
    void Delete(Cart cart);
    void Save();
}
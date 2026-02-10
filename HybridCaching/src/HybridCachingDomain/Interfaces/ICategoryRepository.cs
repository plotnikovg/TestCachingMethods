using HybridCachingDomain.Entities;

namespace HybridCachingDomain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
}
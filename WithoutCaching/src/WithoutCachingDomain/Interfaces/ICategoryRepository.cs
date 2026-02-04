using WithoutCachingDomain.Entities;

namespace WithoutCachingDomain.Interfaces;

public interface ICategoryRepository
{
    Task<Category> GetAllAsync();
    Task<Category> GetByIdAsync(int id);
}
using InMemoryCachingDomain.Entities;

namespace InMemoryCachingDomain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
}
using DistributedCachingRedisDomain.Entities;

namespace DistributedCachingRedisDomain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
}
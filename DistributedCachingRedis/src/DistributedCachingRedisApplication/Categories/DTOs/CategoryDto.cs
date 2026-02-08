namespace DistributedCachingRedisApplication.Categories.DTOs;

public record CategoryDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
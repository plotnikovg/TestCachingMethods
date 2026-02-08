namespace DistributedCachingRedisApplication.Products.DTOs;

public record ProductDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int CategoryId { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsInStock { get; set; }
}
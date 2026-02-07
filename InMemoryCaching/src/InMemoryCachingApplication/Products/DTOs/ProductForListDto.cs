namespace InMemoryCachingApplication.Products.DTOs;

public record ProductForListDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public bool IsInStock { get; set; }
}
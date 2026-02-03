namespace WithoutCachingDomain.Entities;

public class Cart
{
    public int Id { get; set; }
    public List<Product> Products { get; private set; } = new List<Product>();
}
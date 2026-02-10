namespace HybridCachingDomain.Entities;

public class Cart
{
    public int Id { get; set; }
    private List<Product> _products = new List<Product>();
    public IReadOnlyList<Product> Products => _products.AsReadOnly();

    public void AddProduct(Product product)
    {
        if (product == null)
            throw new ArgumentNullException(nameof(product));
        
        if (!product.IsInStock)
            throw new InvalidOperationException("Product is not in the stock");
        
        if (_products.Contains(product))
            throw new InvalidOperationException("Product is already in the cart");
        
        _products.Add(product);
    }
    
    public void RemoveProduct(int productId)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);
        if (product != null)
            _products.Remove(product);
    }
}
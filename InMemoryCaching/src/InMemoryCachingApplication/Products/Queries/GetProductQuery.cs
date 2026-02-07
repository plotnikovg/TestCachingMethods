using InMemoryCachingApplication.Products.DTOs;
using MediatR;
using MediatR.Pipeline;
using InMemoryCachingDomain.Interfaces;

namespace InMemoryCachingApplication.Products.Queries;

public sealed record GetProductQuery : IRequest<ProductDto>
{
    public int ProductId { get; init; }

    public GetProductQuery(int productId)
    {
        ProductId = productId;
    }
}

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    
    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(request.ProductId);

        if (product is null) throw new NullReferenceException("Product not found");

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            CategoryId = product.CategoryId,
            Description = product.Description,
            Price = product.Price,
            IsInStock = product.IsInStock
        };
    }
}
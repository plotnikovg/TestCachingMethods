using DistributedCachingRedisApplication.Products.DTOs;
using MediatR;
using DistributedCachingRedisDomain.Interfaces;

namespace DistributedCachingRedisApplication.Products.Queries;

public sealed record GetProductsQuery : IRequest<List<ProductForListDto>>
{
    public int CategoryId { get; init; }
    public int FirstProductNumber { get; init; }
    public int LastProductNumber { get; init; }

    public GetProductsQuery(int? categoryId, int firstProductNumber, int lastProductNumber)
    {
        CategoryId = categoryId ?? 0;
        FirstProductNumber = firstProductNumber;
        LastProductNumber = lastProductNumber;
    }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductForListDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductForListDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products =
            await _productRepository
                .GetProductsRangeByIdAsync(request.CategoryId, request.FirstProductNumber, request.LastProductNumber);
        
        var productsDto = new List<ProductForListDto>();

        foreach (var product in products)
        {
            productsDto.Add(new ProductForListDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                IsInStock = product.IsInStock
            });
        }
        
        return productsDto;
    }
}
using Moq;
using WithoutCachingApplication.Products.Queries;
using WithoutCachingDomain.Entities;
using WithoutCachingDomain.Interfaces;

namespace Application.Tests;

public class GetProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly GetProductsQueryHandler _handler;

    public GetProductsQueryHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _handler = new GetProductsQueryHandler(_mockProductRepository.Object);
    }

    [Fact]
    public void GetProductsQuery_ShouldSetCategoryIdToZero_WhenNullIsProvided()
    {
        var query = new GetProductsQuery(null, 1, 10);

        Assert.Equal(0, query.CategoryId);
        Assert.Equal(1, query.FirstProductNumber);
        Assert.Equal(10, query.LastProductNumber);
    }

    [Fact]
    public void GetProductsQuery_ShouldSetCategoryId_WhenValueIsProvided()
    {
        var query = new GetProductsQuery(5, 1, 10);

        Assert.Equal(5, query.CategoryId);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedDtos_WhenProductsExist()
    {
        var expectedProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 99.99m, IsInStock = true },
            new Product { Id = 2, Name = "Product 2", Price = 150.00m, IsInStock = false }
        };

        var query = new GetProductsQuery(3, 1, 2);

        _mockProductRepository
            .Setup(repo => repo.GetProductsRangeByIdAsync(3, 1, 2))
            .ReturnsAsync(expectedProducts);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        Assert.Equal(1, result[0].Id);
        Assert.Equal("Product 1", result[0].Name);
        Assert.Equal(99.99m, result[0].Price);
        Assert.True(result[0].IsInStock);

        Assert.Equal(2, result[1].Id);
        Assert.Equal("Product 2", result[1].Name);
        Assert.Equal(150.00m, result[1].Price);
        Assert.False(result[1].IsInStock);

        _mockProductRepository.Verify(repo => repo.GetProductsRangeByIdAsync(3, 1, 2), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoProductsFound()
    {
        var query = new GetProductsQuery(1, 10, 20);

        _mockProductRepository
            .Setup(repo => repo.GetProductsRangeByIdAsync(1, 10, 20))
            .ReturnsAsync(new List<Product>());

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Empty(result);

        _mockProductRepository.Verify(repo => repo.GetProductsRangeByIdAsync(1, 10, 20), Times.Once);
    }
}
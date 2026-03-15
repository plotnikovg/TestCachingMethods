using Moq;
using WithoutCachingApplication.Products.Queries;
using WithoutCachingDomain.Entities;
using WithoutCachingDomain.Interfaces;

namespace Application.Tests;

public class GetProductQueryHandlerTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly GetProductQueryHandler _handler;

    public GetProductQueryHandlerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _handler = new GetProductQueryHandler(_mockProductRepository.Object);
    }

    [Fact]
    public void GetProductQuery_ShouldSetProductId()
    {
        var query = new GetProductQuery(5);

        Assert.Equal(5, query.ProductId);
    }

    [Fact]
    public async Task Handle_ShouldReturnMappedDto_WhenProductExists()
    {
        var expectedProduct = new Product
        {
            Id = 1,
            Name = "Test Product",
            CategoryId = 2,
            Description = "Test Description",
            Price = 49.99m,
            IsInStock = true
        };

        var query = new GetProductQuery(1);

        _mockProductRepository
            .Setup(repo => repo.GetProductByIdAsync(1))
            .ReturnsAsync(expectedProduct);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Product", result.Name);
        Assert.Equal(2, result.CategoryId);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal(49.99m, result.Price);
        Assert.True(result.IsInStock);

        _mockProductRepository.Verify(repo => repo.GetProductByIdAsync(1), Times.Once);
    }
    
    [Fact]
    public async Task Handle_ShouldThrowNullReferenceException_WhenProductDoesNotExist()
    {
        var query = new GetProductQuery(99);

        _mockProductRepository
            .Setup(repo => repo.GetProductByIdAsync(99))
            .ReturnsAsync((Product)null);

        await Assert.ThrowsAsync<NullReferenceException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
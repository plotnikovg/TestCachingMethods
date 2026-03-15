using Moq;
using WithoutCachingApplication.Categories.Queries;
using WithoutCachingDomain.Entities;
using WithoutCachingDomain.Interfaces;


namespace Application.Tests;

public class GetAllCategoriesQueryHandlerTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly GetAllCategoriesQueryHandler _handler;
    
    public GetAllCategoriesQueryHandlerTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _handler = new GetAllCategoriesQueryHandler(_mockCategoryRepository.Object);
    }
    
    [Fact]
    public async Task Handle_ShouldReturnMappedCategoryDtos_WhenCategoriesExist()
    {
        var expectedCategories = new List<Category>
        {
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" }
        };
        
        _mockCategoryRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedCategories);

        var query = new GetAllCategoriesQuery();
        
        var result = await _handler.Handle(query, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        
        Assert.Equal(1, result[0].Id);
        Assert.Equal("Category 1", result[0].Name);
        Assert.Equal(2, result[1].Id);
        Assert.Equal("Category 2", result[1].Name);
        
        _mockCategoryRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoCategoriesExist()
    {
        _mockCategoryRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Category>());

        var query = new GetAllCategoriesQuery();
        
        var result = await _handler.Handle(query, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Empty(result);
        
        _mockCategoryRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }
}
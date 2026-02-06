using MediatR;
using WithoutCachingApplication.Categories.DTOs;
using WithoutCachingDomain.Interfaces;

namespace WithoutCachingApplication.Categories.Queries;

public sealed record GetAllCategoriesQuery : IRequest<List<CategoryDto>>
{
    public int CategoryId { get; init; }
    public string? Name { get; init; }

    public GetAllCategoriesQuery(int categoryId, string? name)
    {
        CategoryId = categoryId;
        Name = name;
    }
}

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync();

        var categoriesDto = new List<CategoryDto>();

        foreach (var category in categories)
        {
            categoriesDto.Add(new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            });
        }
        
        return categoriesDto;
    }
}
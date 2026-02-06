using MediatR;
using Microsoft.AspNetCore.Mvc;
using WithoutCachingApplication.Categories.Queries;

namespace WithoutCachingAPI.Controllers;

public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ISender _mediator;

    public CategoryController(ILogger<CategoryController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll categories called");

        return Ok(await _mediator.Send(new GetAllCategoriesQuery()));
    }
}
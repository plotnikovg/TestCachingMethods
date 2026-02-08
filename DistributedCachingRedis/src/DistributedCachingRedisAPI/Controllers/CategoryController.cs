using MediatR;
using Microsoft.AspNetCore.Mvc;
using DistributedCachingRedisApplication.Categories.Queries;

namespace DistributedCachingRedisAPI.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ISender _mediator;

    public CategoryController(ILogger<CategoryController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("api/[controller]")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GetAll categories called");

        return Ok(await _mediator.Send(new GetAllCategoriesQuery()));
    }
}
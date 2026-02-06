using MediatR;
using Microsoft.AspNetCore.Mvc;
using WithoutCachingApplication.Products.Queries;

namespace WithoutCachingAPI.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;

    private readonly ISender _mediator;

    public ProductController(ILogger<ProductController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, 
        [FromQuery] int firstProductNumber, [FromQuery] int lastProductNumber)
    {
        _logger.LogInformation("GetProducts called");
        
        return Ok(await _mediator.Send(new GetProductsQuery(categoryId, firstProductNumber, lastProductNumber)));
    }

    [HttpGet]
    public async Task<IActionResult> GetProduct([FromQuery] int id)
    {
        _logger.LogInformation("GetProduct called");
        
        return Ok(await _mediator.Send(new GetProductQuery(id)));
    }
}
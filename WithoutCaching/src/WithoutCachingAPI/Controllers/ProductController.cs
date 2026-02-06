using MediatR;
using Microsoft.AspNetCore.Mvc;
using WithoutCachingApplication.Products.DTOs;
using WithoutCachingApplication.Products.Queries;

namespace WithoutCachingAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;

    private readonly ISender _mediator;

    public ProductController(ILogger<ProductController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProductForListDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("GetRange")]
    public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, 
        [FromQuery] int firstProductNumber, [FromQuery] int lastProductNumber)
    {
        _logger.LogInformation("GetProducts called");
        
        return Ok(await _mediator.Send(new GetProductsQuery(categoryId, firstProductNumber, lastProductNumber)));
    }

    [HttpGet]
    [Route("Get")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProduct([FromQuery] int id)
    {
        _logger.LogInformation("GetProduct called");
        
        return Ok(await _mediator.Send(new GetProductQuery(id)));
    }
}
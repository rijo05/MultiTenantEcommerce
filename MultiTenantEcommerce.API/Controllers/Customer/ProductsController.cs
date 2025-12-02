using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Catalog.Products.Queries.GetById;
using MultiTenantEcommerce.Application.Catalog.Products.Queries.GetFiltered;
using MultiTenantEcommerce.Application.Catalog.Products.Queries.GetImage;

namespace MultiTenantEcommerce.API.Controllers.Customer;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ProductResponseDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<IProductDTO>>> GetProducts([FromQuery] ProductFilterDTO productFilter)
    {
        var query = new GetFilteredProductsQuery(
            productFilter.CategoryName,
            productFilter.CategoryId,
            productFilter.Name,
            productFilter.MinPrice,
            productFilter.MaxPrice,
            true,  //isActive
            false, //isAdmin
            productFilter.Page,
            productFilter.PageSize,
            productFilter.Sort);

        var products = await _mediator.Send(query);

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<IProductDTO>> GetById(Guid id)
    {
        var query = new GetProductByIdQuery(id, false);
        var product = await _mediator.Send(query);

        return Ok(product);
    }

    [HttpGet("get-image/{productId:guid}/{imageId:guid}")]
    [ProducesResponseType(typeof(ProductImageResponseDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<IProductImageDTO>> GetImage(Guid productId, Guid imageId)
    {
        var query = new GetProductImageByIdQuery(productId, imageId, false);

        var image = await _mediator.Send(query);

        return Ok(image);
    }
}

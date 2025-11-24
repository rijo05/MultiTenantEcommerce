using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Catalog.Products.Queries.GetById;
using MultiTenantEcommerce.Application.Catalog.Products.Queries.GetFiltered;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;

namespace MultiTenantEcommerce.API.Controllers.Customer;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFileStorageService _fileStorageService;

    public ProductsController(IMediator mediator, IFileStorageService fileStorageService)
    {
        _mediator = mediator;
        _fileStorageService = fileStorageService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponseDTO>>> GetProducts([FromQuery] ProductFilterDTO productFilter)
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

        return Ok(products.Cast<ProductResponseDTO>());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponseDTO>> GetById(Guid id)
    {
        var query = new GetProductByIdQuery(id, false);
        var product = await _mediator.Send(query);

        return Ok(product);
    }

    [HttpGet("get-image/{productId:guid}/{key}")]
    public async Task<ActionResult<string>> GetByImage(Guid productId, string key)
    {
        return _fileStorageService.GetImageUrl(key);
    }
}

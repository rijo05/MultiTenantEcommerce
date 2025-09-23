using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Create;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Delete;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Update;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.Queries.GetById;
using MultiTenantEcommerce.Application.Catalog.Products.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers.Admin;


[ApiController]
[Authorize(Policy = "EmployeeOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.product")]
    [HttpGet]
    public async Task<ActionResult<List<ProductResponseAdminDTO>>> GetProducts([FromQuery] ProductFilterAdminDTO productFilter)
    {
        var query = new GetFilteredProductsQuery(
            productFilter.CategoryName,
            productFilter.CategoryId,
            productFilter.Name,
            productFilter.MinPrice,
            productFilter.MaxPrice,
            productFilter.IsActive,
            true,
            productFilter.Page,
            productFilter.PageSize,
            productFilter.Sort);

        var products = await _mediator.Send(query);

        return Ok(products.Cast<ProductResponseAdminDTO>());
    }

    [HasPermission("read.product")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponseAdminDTO>> GetById(Guid id)
    {
        var query = new GetProductByIdQuery(id, true);
        var product = await _mediator.Send(query);

        return Ok(product);
    }

    [HasPermission("create.product")]
    [HttpPost]
    public async Task<ActionResult<ProductResponseAdminDTO>> Create([FromBody] CreateProductCommand command)
    {
        if (command is null)
            return BadRequest("Product data must be provided.");

        var product = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product
            );
    }

    [HasPermission("update.product")]
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<ProductResponseWithoutStockAdminDTO>> Update(Guid id, [FromBody] UpdateProductDTO productDTO)
    {
        if (productDTO is null)
            return BadRequest("Product data must be provided.");

        var command = new UpdateProductCommand(id,
            productDTO.Name,
            productDTO.Description,
            productDTO.Price,
            productDTO.IsActive,
            productDTO.CategoryId);

        var product = await _mediator.Send(command);
        return Ok(product);
    }

    [HasPermission("delete.product")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProductCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}

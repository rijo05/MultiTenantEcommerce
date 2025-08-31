using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Create;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Delete;
using MultiTenantEcommerce.Application.Catalog.Products.Commands.Update;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Catalog.Products.Queries.GetById;
using MultiTenantEcommerce.Application.Catalog.Products.Queries.GetFiltered;
using MultiTenantEcommerce.Application.Common.Interfaces;

namespace MultiTenantEcommerce.API.Controllers;

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
    public async Task<ActionResult<List<ProductResponseDTO>>> GetProducts([FromQuery] GetFilteredProductsQuery filteredQuery)
    {
        var products = await _mediator.Send(filteredQuery);

        return Ok(products);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponseDTO>> GetById(Guid id)
    {
        var query = new GetProductByIdQuery(id);
        var product = await _mediator.Send(query);

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDTO>> Create([FromBody] CreateProductCommand command)
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

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<ProductResponseDTO>> Update(Guid id, [FromBody] UpdateProductDTO productDTO)
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProductCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.Create;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.ManageImages;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.Update;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Images;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Queries.GetFilteredAdmin;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "TenantMemberOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    #region PRODUCT

    [HasPermission("read.product")]
    [HttpGet]
    [ProducesResponseType(typeof(List<ProductResponseAdminDTO>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<IProductDTO>>> GetProducts([FromQuery] ProductFilterAdminDTO productFilter)
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

        return Ok(products);
    }

    [HasPermission("read.product")]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductResponseAdminDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<IProductDTO>> GetById(Guid id)
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
    public async Task<ActionResult<ProductResponseAdminDTO>> Update(Guid id, [FromBody] UpdateProductDTO productDTO)
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

    #endregion

    #region PRODUCT IMAGE

    [HasPermission("create.product")]
    [HttpPost("add-image/{productId:guid}")]
    public async Task<ActionResult<List<PresignedUpload>>> AddImage(Guid productId,
        [FromBody] List<ProductImageMetadataDTO> metadataDTO)
    {
        var command = new AddProductImagesCommand(productId, metadataDTO);
        var uploads = await _mediator.Send(command);

        return uploads;
    }


    [HasPermission("delete.product")]
    [HttpDelete("image/{productId:guid}/{imageId:guid}")]
    public async Task<IActionResult> DeleteImage(Guid productId, Guid imageId)
    {
        var command = new DeleteProductImageCommand(productId, imageId);
        await _mediator.Send(command);

        return NoContent();
    }

    [HasPermission("read.product")]
    [HttpGet("get-image/{productId:guid}/{imageId:guid}")]
    [ProducesResponseType(typeof(ProductImageResponseAdminDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<IProductImageDTO>> GetImage(Guid productId, Guid imageId)
    {
        var query = new GetProductImageByIdQuery(productId, imageId, true);
        var image = await _mediator.Send(query);

        return Ok(image);
    }

    #endregion
}
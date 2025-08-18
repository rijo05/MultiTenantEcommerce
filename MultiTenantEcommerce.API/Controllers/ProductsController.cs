using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.DTOs.Product;
using MultiTenantEcommerce.Application.Interfaces;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }


    [HttpGet]
    public async Task<ActionResult<List<ProductResponseDTO>>> GetProducts([FromQuery] ProductFilterDTO productFilterDTO)
    {
        var products = await _productService.GetFilteredProductsAsync(productFilterDTO);

        return Ok(products);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponseDTO>> GetById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        return Ok(product);
    }


    [HttpPost]
    public async Task<ActionResult<ProductResponseDTO>> Create([FromBody] CreateProductDTO productDTO)
    {
        if (productDTO is null)
            return BadRequest("Product data must be provided.");

        var product = await _productService.AddProductAsync(productDTO);

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

        var product = await _productService.UpdateProductAsync(id, productDTO);
        return Ok(product);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productService.DeleteProductAsync(id);
        return NoContent();
    }
}

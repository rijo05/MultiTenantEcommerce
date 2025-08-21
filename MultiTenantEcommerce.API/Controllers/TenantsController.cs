using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.DTOs.Product;
using MultiTenantEcommerce.Application.DTOs.Tenant;
using MultiTenantEcommerce.Application.Interfaces;
using MultiTenantEcommerce.Application.Services;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly ITenantService _tenantService;

    public TenantsController(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TenantResponseDTO>>> GetProducts([FromQuery] TenantFilterDTO tenantFilterDTO)
    {
        var products = await _tenantService.GetFilteredTenantsAsync(tenantFilterDTO);

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TenantResponseDTO>> GetById(Guid id)
    {
        var product = await _tenantService.GetTenantByIdAsync(id);

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<TenantResponseDTO>> Create([FromBody] CreateTenantDTO tenantDTO)
    {
        if (tenantDTO is null)
            return BadRequest("Tenant data must be provided.");

        var tenant = await _tenantService.AddTenantAsync(tenantDTO);

        return CreatedAtAction(
            nameof(GetById),
            new { id = tenant.TenantId },
            tenant
            );
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<TenantResponseDTO>> Update(Guid id, [FromBody] UpdateTenantDTO tenantDTO)
    {
        if (tenantDTO is null)
            return BadRequest("Product data must be provided.");

        var tenant = await _tenantService.UpdateTenantAsync(id, tenantDTO);
        return Ok(tenant);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _tenantService.DeleteTenantAsync(id);
        return NoContent();
    }
}

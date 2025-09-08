using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Delete;
using MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Update;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
using MultiTenantEcommerce.Application.Tenants.Queries.Tenant.GetById;
using MultiTenantEcommerce.Application.Tenants.Queries.Tenant.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<TenantResponseDTO>>> GetTenants([FromQuery] GetFilteredTenantsQuery filteredQuery)
    {
        var tenants = await _mediator.Send(filteredQuery);

        return Ok(tenants);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TenantResponseDTO>> GetById(Guid id)
    {
        var query = new GetTenantByIdQuery(id);
        var tenant = await _mediator.Send(query);

        return Ok(tenant);
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<TenantResponseDTO>> Update(Guid id, [FromBody] UpdateTenantDTO tenantDTO)
    {
        if (tenantDTO is null)
            return BadRequest("Tenant data must be provided.");

        var command = new UpdateTenantCommand(id, tenantDTO.CompanyName);

        var tenant = await _mediator.Send(command);
        return Ok(tenant);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteTenantCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}

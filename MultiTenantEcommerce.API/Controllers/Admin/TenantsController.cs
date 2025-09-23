using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.API.Extensions;
using MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Delete;
using MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Update;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
using MultiTenantEcommerce.Application.Tenants.Queries.Tenant.GetById;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "EmployeeOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class TenantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.tenant")]
    [HttpGet("me")]
    public async Task<ActionResult<TenantResponseDTO>> GetTenant()
    {
        var tenantId = User.GetTenantId();

        var query = new GetTenantByIdQuery(tenantId);
        var tenants = await _mediator.Send(query);

        return Ok(tenants);
    }

    [HasPermission("update.tenant")]
    [HttpPatch("me")]
    public async Task<ActionResult<TenantResponseDTO>> Update([FromBody] UpdateTenantDTO tenantDTO)
    {
        if (tenantDTO is null)
            return BadRequest("Tenant data must be provided.");

        var tenantId = User.GetTenantId();

        var command = new UpdateTenantCommand(tenantId, tenantDTO.CompanyName);

        var tenant = await _mediator.Send(command);
        return Ok(tenant);
    }

    [HasPermission("delete.tenant")]
    [HttpDelete("me")]
    public async Task<IActionResult> Delete()
    {
        var tenantId = User.GetTenantId();

        var command = new DeleteTenantCommand(tenantId);
        await _mediator.Send(command);

        return NoContent();
    }
}

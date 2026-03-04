using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "TenantMemberOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.permission")]
    [HttpGet]
    public async Task<ActionResult<List<PermissionResponseDTO>>> GetAll()
    {
        var query = new GetAllPermissionsQuery();

        var permissions = await _mediator.Send(query);

        return Ok(permissions);
    }

    [HasPermission("read.permission")]
    [HttpGet("by-action/{actionName}")]
    public async Task<ActionResult<List<PermissionResponseDTO>>> GetPermissionByAction(string actionName)
    {
        var query = new GetPermissionByActionQuery(actionName);

        var permissions = await _mediator.Send(query);

        return Ok(permissions);
    }

    [HasPermission("read.permission")]
    [HttpGet("by-area/{areaName}")]
    public async Task<ActionResult<List<PermissionResponseDTO>>> GetPermissionByArea(string areaName)
    {
        var query = new GetPermissionByAreaQuery(areaName);

        var permissions = await _mediator.Send(query);

        return Ok(permissions);
    }
}
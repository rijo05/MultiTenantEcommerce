using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Users.Permissions.Commands.AddToRole;
using MultiTenantEcommerce.Application.Users.Permissions.Commands.CreateRole;
using MultiTenantEcommerce.Application.Users.Permissions.Commands.DeleteRole;
using MultiTenantEcommerce.Application.Users.Permissions.Commands.RemoveFromRole;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Queries.GetAll;
using MultiTenantEcommerce.Application.Users.Permissions.Queries.GetById;
using MultiTenantEcommerce.Application.Users.Permissions.Queries.GetByName;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoleResponseDTO>>> GetRoles([FromQuery] GetAllRolesQuery rolesQuery)
    {
        var roles = await _mediator.Send(rolesQuery);

        return Ok(roles);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoleResponseDTO>> GetById(Guid id)
    {
        var query = new GetRoleByIdQuery(id);
        var roles = await _mediator.Send(query);

        return Ok(roles);
    }

    [HttpGet("name/{name}")]
    public async Task<ActionResult<RoleResponseDTO>> GetByName(string name)
    {
        var query = new GetRoleByNameQuery(name);
        var role = await _mediator.Send(query);

        return Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult<RoleResponseDTO>> CreateRole([FromBody] CreateRoleCommand roleCommand)
    {
        var role = await _mediator.Send(roleCommand);

        return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
    }

    [HttpPatch("{id:guid}/permissions/add")]
    public async Task<ActionResult<RoleResponseDTO>> AddToRole(Guid id, [FromBody] List<Guid> permissions)
    {
        var command = new AddPermissionsToRoleCommand(id, permissions);
        var role = await _mediator.Send(command);

        return Ok(role);
    }

    [HttpPatch("{id:guid}/permissions/remove")]
    public async Task<ActionResult<RoleResponseDTO>> RemoveFromRole(Guid id, [FromBody] List<Guid> permissions)
    {
        var command = new RemovePermissionsFromRoleCommand(id, permissions);
        var role = await _mediator.Send(command);

        return Ok(role);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        var command = new DeleteRoleCommand(id);
        var role = await _mediator.Send(command);

        return NoContent();
    }
}

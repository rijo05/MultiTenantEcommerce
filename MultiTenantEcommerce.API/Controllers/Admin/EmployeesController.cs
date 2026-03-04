using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.API.Extensions;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "TenantMemberOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class TenantMembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public TenantMembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.employee")]
    [HttpGet]
    public async Task<ActionResult<List<TenantMemberResponseDTO>>> GetTenantMembers(
        [FromQuery] GetFilteredTenantMembersQuery TenantMemberFilter)
    {
        var employees = await _mediator.Send(TenantMemberFilter);

        return Ok(employees);
    }

    [HasPermission("read.employee")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TenantMemberResponseDTO>> GetById(Guid id)
    {
        var query = new GetTenantMemberByIdQuery(id);
        var employee = await _mediator.Send(query);

        return Ok(employee);
    }

    [HttpGet("me")]
    public async Task<ActionResult<TenantMemberResponseDTO>> GetMyProfile()
    {
        var userId = User.GetUserId();
        var query = new GetTenantMemberByIdQuery(userId);

        var employee = await _mediator.Send(query);

        return Ok(employee);
    }

    [HasPermission("read.employee")]
    [HttpGet("email/{email}")]
    public async Task<ActionResult<TenantMemberResponseDTO>> GetByEmail(string email)
    {
        var query = new GetTenantMemberByEmailQuery(email);
        var employee = await _mediator.Send(query);

        return Ok(employee);
    }


    [HttpPatch("me")]
    public async Task<ActionResult<TenantMemberResponseDTO>> UpdateMyProfile(
        [FromBody] UpdateTenantMemberDTO TenantMemberDTO)
    {
        var userId = User.GetUserId();

        var command = new UpdateTenantMemberCommand(userId,
            TenantMemberDTO.Name,
            TenantMemberDTO.Email,
            TenantMemberDTO.Password,
            null,
            null);

        var employee = await _mediator.Send(command);
        return Ok(employee);
    }

    [HasPermission("update.employee")]
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<TenantMemberResponseDTO>> UpdateProfile(Guid id,
        [FromBody] UpdateTenantMemberAdminDTO TenantMemberDTO)
    {
        var command = new UpdateTenantMemberCommand(id,
            TenantMemberDTO.Name,
            TenantMemberDTO.Email,
            TenantMemberDTO.Password,
            TenantMemberDTO.Role,
            TenantMemberDTO.IsActive);


        var employee = await _mediator.Send(command);
        return Ok(employee);
    }

    [HasPermission("delete.employee")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteTenantMemberCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HasPermission("assign.role")]
    [HttpPost("{id:guid}/roles")]
    public async Task<ActionResult<TenantMemberResponseDTO>> AssignRole(Guid id, [FromBody] List<Guid> roles)
    {
        var command = new AssignRoleToTenantMemberCommand(id, roles);

        var employee = await _mediator.Send(command);
        return Ok(employee);
    }

    [HasPermission("remove.role")]
    [HttpDelete("{id:guid}/roles")]
    public async Task<ActionResult<TenantMemberResponseDTO>> RemoveRole(Guid id, [FromBody] List<Guid> roles)
    {
        var command = new RemoveRoleFromTenantMemberCommand(id, roles);

        var employee = await _mediator.Send(command);
        return Ok(employee);
    }
}
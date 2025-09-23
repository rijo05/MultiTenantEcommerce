using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.API.Extensions;
using MultiTenantEcommerce.Application.Users.Employees.Commands.AssignRole;
using MultiTenantEcommerce.Application.Users.Employees.Commands.Delete;
using MultiTenantEcommerce.Application.Users.Employees.Commands.RemoveRole;
using MultiTenantEcommerce.Application.Users.Employees.Commands.Update;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Queries.GetByEmail;
using MultiTenantEcommerce.Application.Users.Employees.Queries.GetById;
using MultiTenantEcommerce.Application.Users.Employees.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "EmployeeOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.employee")]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponseDTO>>> GetEmployees([FromQuery] GetFilteredEmployeesQuery EmployeeFilter)
    {
        var employees = await _mediator.Send(EmployeeFilter);

        return Ok(employees);
    }

    [HasPermission("read.employee")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeResponseDTO>> GetById(Guid id)
    {
        var query = new GetEmployeeByIdQuery(id);
        var employee = await _mediator.Send(query);

        return Ok(employee);
    }

    [HttpGet("me")]
    public async Task<ActionResult<EmployeeResponseDTO>> GetMyProfile()
    {
        var userId = User.GetUserId();
        var query = new GetEmployeeByIdQuery(userId);

        var employee = await _mediator.Send(query);

        return Ok(employee);
    }

    [HasPermission("read.employee")]
    [HttpGet("email/{email}")]
    public async Task<ActionResult<EmployeeResponseDTO>> GetByEmail(string email)
    {
        var query = new GetEmployeeByEmailQuery(email);
        var employee = await _mediator.Send(query);

        return Ok(employee);
    }


    [HttpPatch("me")]
    public async Task<ActionResult<EmployeeResponseDTO>> UpdateMyProfile([FromBody] UpdateEmployeeDTO EmployeeDTO)
    {
        var userId = User.GetUserId();

        var command = new UpdateEmployeeCommand(userId,
            EmployeeDTO.Name,
            EmployeeDTO.Email,
            EmployeeDTO.Password,
            null,
            null);

        var employee = await _mediator.Send(command);
        return Ok(employee);
    }

    [HasPermission("update.employee")]
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<EmployeeResponseDTO>> UpdateProfile(Guid id, [FromBody] UpdateEmployeeAdminDTO EmployeeDTO)
    {
        var command = new UpdateEmployeeCommand(id,
            EmployeeDTO.Name,
            EmployeeDTO.Email,
            EmployeeDTO.Password,
            EmployeeDTO.Role,
            EmployeeDTO.IsActive);


        var employee = await _mediator.Send(command);
        return Ok(employee);
    }

    [HasPermission("delete.employee")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteEmployeeCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HasPermission("assign.role")]
    [HttpPost("{id:guid}/roles")]
    public async Task<ActionResult<EmployeeResponseDTO>> AssignRole(Guid id, [FromBody] List<Guid> roles)
    {
        var command = new AssignRoleToEmployeeCommand(id, roles);

        var employee = await _mediator.Send(command);
        return Ok(employee);
    }

    [HasPermission("remove.role")]
    [HttpDelete("{id:guid}/roles")]
    public async Task<ActionResult<EmployeeResponseDTO>> RemoveRole(Guid id, [FromBody] List<Guid> roles)
    {
        var command = new RemoveRoleFromEmployeeCommand(id, roles);

        var employee = await _mediator.Send(command);
        return Ok(employee);
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Users.Employees.Commands.AssignRole;
using MultiTenantEcommerce.Application.Users.Employees.Commands.Delete;
using MultiTenantEcommerce.Application.Users.Employees.Commands.RemoveRole;
using MultiTenantEcommerce.Application.Users.Employees.Commands.Update;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Queries.GetByEmail;
using MultiTenantEcommerce.Application.Users.Employees.Queries.GetById;
using MultiTenantEcommerce.Application.Users.Employees.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponseDTO>>> GetEmployees([FromQuery] GetFilteredEmployeesQuery EmployeeFilter)
    {
        var employees = await _mediator.Send(EmployeeFilter);

        return Ok(employees);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeResponseDTO>> GetById(Guid id)
    {
        var query = new GetEmployeeByIdQuery(id);
        var employee = await _mediator.Send(query);

        return Ok(employee);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<EmployeeResponseDTO>> GetByEmail(string email)
    {
        var query = new GetEmployeeByEmailQuery(email);
        var employee = await _mediator.Send(query);

        return Ok(employee);
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<EmployeeResponseDTO>> Update(Guid id, [FromBody] UpdateEmployeeDTO EmployeeDTO)
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


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteEmployeeCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:guid}/roles/add")]
    public async Task<ActionResult<EmployeeResponseDTO>> AssignRole(Guid id, [FromBody] List<Guid> roles)
    {
        var command = new AssignRoleToEmployeeCommand(id, roles);

        var employee = await _mediator.Send(command);
        return Ok(employee);
    }

    [HttpDelete("{id:guid}/roles/remove")]
    public async Task<ActionResult<EmployeeResponseDTO>> RemoveRole(Guid id, [FromBody] List<Guid> roles)
    {
        var command = new RemoveRoleFromEmployeeCommand(id, roles);

        var employee = await _mediator.Send(command);
        return Ok(employee);
    }


}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Employees.Commands.Create;
using MultiTenantEcommerce.Application.Users.Employees.Commands.Delete;
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


    [HttpPost]
    public async Task<ActionResult<EmployeeResponseDTO>> Create([FromBody] CreateEmployeeCommand command)
    {
        if (command is null)
            return BadRequest("Employee data must be provided.");

        var employee = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { id = employee.Id },
            employee
            );
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
}

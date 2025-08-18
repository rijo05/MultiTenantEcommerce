using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.DTOs.Employees;
using MultiTenantEcommerce.Application.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _EmployeeService;

    public EmployeesController(IEmployeeService EmployeeService)
    {
        _EmployeeService = EmployeeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeResponseDTO>>> GetEmployees([FromQuery] EmployeeFilterDTO EmployeeFilter)
    {
        var Employees = await _EmployeeService.GetFilteredEmployeesAsync(EmployeeFilter);

        return Ok(Employees);
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeResponseDTO>> GetById(Guid id)
    {
        var Employee = await _EmployeeService.GetEmployeeByIdAsync(id);

        return Ok(Employee);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<EmployeeResponseDTO>> GetByEmail(string email)
    {
        var Employee = await _EmployeeService.GetEmployeeByEmailAsync(email);

        return Ok(Employee);
    }


    [HttpPost]
    public async Task<ActionResult<EmployeeResponseDTO>> Create([FromBody] CreateEmployeeDTO EmployeeDTO)
    {
        if (EmployeeDTO is null)
            return BadRequest("Employee data must be provided.");

        var Employee = await _EmployeeService.AddEmployeeAsync(EmployeeDTO);

        return CreatedAtAction(
            nameof(GetById),
            new { id = Employee.Id },
            Employee
            );
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<EmployeeResponseDTO>> Update(Guid id, [FromBody] UpdateEmployeeDTO EmployeeDTO)
    {
        var Employee = await _EmployeeService.UpdateEmployeeAsync(id, EmployeeDTO);
        return Ok(Employee);
    }
    

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _EmployeeService.DeleteEmployeeAsync(id);
        return NoContent();
    }
}

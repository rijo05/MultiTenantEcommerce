using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Users.Customers.Commands.Delete;
using MultiTenantEcommerce.Application.Users.Customers.Commands.Update;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Application.Users.Customers.Queries.GetByEmail;
using MultiTenantEcommerce.Application.Users.Customers.Queries.GetById;
using MultiTenantEcommerce.Application.Users.Customers.Queries.GetByPhoneNumber;
using MultiTenantEcommerce.Application.Users.Customers.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "EmployeeOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.customer")]
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponseDTO>>> GetCustomers([FromQuery] GetFilteredCustomerQuery customerQuery)
    {
        var customers = await _mediator.Send(customerQuery);

        return Ok(customers);
    }

    [HasPermission("read.customer")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetById(Guid id)
    {
        var query = new GetCustomerByIdQuery(id);
        var customer = await _mediator.Send(query);

        return Ok(customer);
    }

    [HasPermission("read.customer")]
    [HttpGet("email/{email}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetByEmail(string email)
    {
        var query = new GetCustomerByEmailQuery(email);
        var customer = await _mediator.Send(query);

        return Ok(customer);
    }

    [HasPermission("read.customer")]
    [HttpGet("phonenumber/{number}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetByPhoneNumber(string number, string countryCode)
    {
        var query = new GetCustomerByPhoneNumberQuery(countryCode, number);
        var customer = await _mediator.Send(query);

        return Ok(customer);
    }

    [HasPermission("update.customer")]
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<CustomerResponseDTO>> Update(Guid id, [FromBody] UpdateCustomerDTO customerDTO)
    {
        var command = new UpdateCustomerCommand(id,
            customerDTO.Name,
            customerDTO.Email,
            customerDTO.Password,
            customerDTO.CountryCode,
            customerDTO.PhoneNumber,
            customerDTO.Address);

        var customer = await _mediator.Send(command);
        return Ok(customer);
    }

    [HasPermission("delete.customer")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCustomerCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}

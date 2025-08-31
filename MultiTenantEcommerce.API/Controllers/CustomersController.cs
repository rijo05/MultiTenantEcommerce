using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Customers.Commands.Create;
using MultiTenantEcommerce.Application.Users.Customers.Commands.Delete;
using MultiTenantEcommerce.Application.Users.Customers.Commands.Update;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Application.Users.Customers.Queries.GetByEmail;
using MultiTenantEcommerce.Application.Users.Customers.Queries.GetById;
using MultiTenantEcommerce.Application.Users.Customers.Queries.GetByPhoneNumber;
using MultiTenantEcommerce.Application.Users.Customers.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase 
{
    private IMediator _mediator;

    public CustomersController(IMediator mediator) 
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<ActionResult<List<CustomerResponseDTO>>> GetCustomers([FromQuery] GetFilteredCustomerQuery customerQuery)
    {
        var customers = await _mediator.Send(customerQuery);

        return Ok(customers);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetById(Guid id)
    {
        var query = new GetCustomerByIdQuery(id);
        var customer = await _mediator.Send(query);

        return Ok(customer);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetByEmail(string email)
    {
        var query = new GetCustomerByEmailQuery(email);
        var customer = await _mediator.Send(query);

        return Ok(customer);
    }

    [HttpGet("phonenumber/{number}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetByPhoneNumber(string number, string countryCode)
    {
        var query = new GetCustomerByPhoneNumberQuery(countryCode, number);
        var customer = await _mediator.Send(query);

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponseDTO>> Create([FromBody] CreateCustomerCommand command)
    {
        if(command == null)
            return BadRequest("Customer data must be provided.");

        var customer = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetById),
            new { id = customer.Id },
            customer
            );
    }

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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCustomerCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}

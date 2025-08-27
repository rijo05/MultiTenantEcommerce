using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.DTOs;
using MultiTenantEcommerce.Application.DTOs.Customer;
using MultiTenantEcommerce.Application.Interfaces;
using MultiTenantEcommerce.Application.Services;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase 
{
    private ICustomerService _customerService;

    public CustomersController(ICustomerService customerService) 
    {
        _customerService = customerService;
    }


    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetById(Guid id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);

        return Ok(customer);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetByEmail(string email)
    {
        var customer = await _customerService.GetCustomerByEmailAsync(email);

        return Ok(customer);
    }

    [HttpGet("phonenumber/{number}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetByPhoneNumber(string number)
    {
        var customer = await _customerService.GetCustomerByPhoneNumberAsync(number);

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponseDTO>> Create([FromBody] CreateCustomerDTO customerDTO)
    {
        if(customerDTO == null)
            return BadRequest("Customer data must be provided.");

        var customer = await _customerService.AddCustomerAsync(customerDTO);

        return CreatedAtAction(
            nameof(GetById),
            new { id = customer.Id },
            customer
            );
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<CustomerResponseDTO>> Update(Guid id, [FromBody] UpdateCustomerDTO customerDTO)
    {
        var customer = await _customerService.UpdateCustomerAsync(id, customerDTO);
        return Ok(customer);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _customerService.DeleteCustomerAsync(id);
        return Ok();
    }
}

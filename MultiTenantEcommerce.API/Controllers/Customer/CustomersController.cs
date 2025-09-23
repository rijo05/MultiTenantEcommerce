using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Extensions;
using MultiTenantEcommerce.Application.Users.Customers.Commands.Delete;
using MultiTenantEcommerce.Application.Users.Customers.Commands.Update;
using MultiTenantEcommerce.Application.Users.Customers.DTOs;
using MultiTenantEcommerce.Application.Users.Customers.Queries.GetById;

namespace MultiTenantEcommerce.API.Controllers.Customer;

[Authorize(Policy = "CustomerOnly")]
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("me")]
    public async Task<ActionResult<CustomerResponseDTO>> GetMyProfile()
    {
        var claims = HttpContext.User.Claims.ToList();
        foreach (var claim in claims)
        {
            Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
        }

        var userId = User.GetUserId();

        var query = new GetCustomerByIdQuery(userId);
        var customer = await _mediator.Send(query);

        return Ok(customer);
    }


    [HttpPatch("me")]
    public async Task<ActionResult<CustomerResponseDTO>> UpdateMyProfile([FromBody] UpdateCustomerDTO customerDTO)
    {
        var userId = User.GetUserId();

        var command = new UpdateCustomerCommand(userId,
            customerDTO.Name,
            customerDTO.Email,
            customerDTO.Password,
            customerDTO.CountryCode,
            customerDTO.PhoneNumber,
            customerDTO.Address);

        var customer = await _mediator.Send(command);
        return Ok(customer);
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMyProfile()
    {
        var userId = User.GetUserId();

        var command = new DeleteCustomerCommand(userId);
        await _mediator.Send(command);
        return NoContent();
    }
}

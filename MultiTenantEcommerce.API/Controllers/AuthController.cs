using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Auth.Commands.CreateCustomer;
using MultiTenantEcommerce.Application.Auth.Commands.CreateEmployee;
using MultiTenantEcommerce.Application.Auth.Commands.CreateTenant;
using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Auth.Queries.LoginCustomer;
using MultiTenantEcommerce.Application.Auth.Queries.LoginEmployee;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register/customer")]
    public async Task<ActionResult<AuthCustomerResponseDTO>> RegisterCustomer([FromBody] CreateCustomerCommand customerCommand)
    {
        var result = await _mediator.Send(customerCommand);

        return Ok(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("register/employee")]
    public async Task<ActionResult<AuthEmployeeResponseDTO>> RegisterEmployee([FromBody] CreateEmployeeCommand employeeCommand)
    {
        var result = await _mediator.Send(employeeCommand);

        return Ok(result);
    }
    [HttpPost("register/tenant")]
    public async Task<ActionResult<AuthTenantResponse>> RegisterTenant([FromBody] CreateTenantCommand tenantCommand)
    {
        var result = await _mediator.Send(tenantCommand);

        return Ok(result);
    }

    [HttpPost("login/customer")]
    public async Task<ActionResult<AuthCustomerResponseDTO>> LoginCustomer([FromBody] LoginCustomerQuery loginCustomer)
    {
        var result = await _mediator.Send(loginCustomer);

        return Ok(result);
    }

    [HttpPost("login/employee")]
    public async Task<ActionResult<AuthEmployeeResponseDTO>> LoginEmployee([FromBody] LoginEmployeeQuery loginEmployee)
    {
        var result = await _mediator.Send(loginEmployee);

        return Ok(result);
    }
}

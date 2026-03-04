using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Commerce.Customers.Commands.Auth.CreateCustomer;
using MultiTenantEcommerce.Application.Commerce.Customers.Commands.Auth.LoginCustomer;
using MultiTenantEcommerce.Application.Platform.Identity.Commands.LoginTenant;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Commands.CreateMember;
using MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.CreateTenant;

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

    [HttpPost("customer/register")]
    public async Task<ActionResult<AuthCustomerResponseDTO>> RegisterCustomer(
        [FromBody] CreateCustomerCommand customerCommand)
    {
        var result = await _mediator.Send(customerCommand);

        return Ok(result);
    }

    [HttpPost("customers/login")]
    public async Task<ActionResult<AuthCustomerResponseDTO>> LoginCustomer([FromBody] LoginCustomerQuery loginCustomer)
    {
        var result = await _mediator.Send(loginCustomer);

        return Ok(result);
    }

    [HasPermission("create.employee")]
    [Authorize(Policy = "Permission")] //############
    [HttpPost("employees/register")]
    public async Task<ActionResult<AuthTenantMemberResponseDTO>> RegisterTenantMember(
        [FromBody] CreateTenantMemberCommand employeeCommand)
    {
        var result = await _mediator.Send(employeeCommand);

        return Ok(result);
    }

    [HttpPost("employees/login")]
    public async Task<ActionResult<AuthTenantMemberResponseDTO>> LoginTenantMember(
        [FromBody] LoginTenantMemberQuery loginTenantMember)
    {
        var result = await _mediator.Send(loginTenantMember);

        return Ok(result);
    }

    [HttpPost("tenants/register")]
    public async Task<ActionResult<string>> RegisterTenant([FromBody] CreateTenantCommand tenantCommand)
    {
        var result = await _mediator.Send(tenantCommand);

        return Ok(result);
    }
}
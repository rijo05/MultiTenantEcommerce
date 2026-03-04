using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "TenantMemberOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.cart")]
    [HttpGet("customer/{id:guid}")]
    public async Task<ActionResult<CartResponseDTO>> GetByCustomerId(Guid id)
    {
        var query = new GetCartByCustomerIdQuery(id);
        var cart = await _mediator.Send(query);

        return Ok(cart);
    }
}
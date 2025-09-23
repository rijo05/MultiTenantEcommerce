using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Extensions;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Sales.Orders.Queries.GetByCustomerId;
using MultiTenantEcommerce.Application.Sales.Orders.Queries.GetById;

namespace MultiTenantEcommerce.API.Controllers.Customer;

[Authorize(Policy = "CustomerOnly")]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("me")]
    public async Task<ActionResult<List<OrderResponseDTO>>> GetMyOrders()
    {
        var userId = User.GetUserId();

        var command = new GetOrdersByCustomerIdQuery(userId);

        var orders = await _mediator.Send(command);

        return Ok(orders);
    }

    [HttpGet("me/{id:guid}")]
    public async Task<ActionResult<OrderResponseDTO>> GetMyOrderById(Guid id)
    {
        var userId = User.GetUserId();

        var command = new GetOrderByIdQuery(userId, id);

        var orders = await _mediator.Send(command);

        return Ok(orders);
    }
}
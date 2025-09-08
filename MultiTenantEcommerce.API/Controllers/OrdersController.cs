using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Sales.Orders.Commands.ChangeStatus;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Sales.Orders.Queries.GetByCustomerId;
using MultiTenantEcommerce.Application.Sales.Orders.Queries.GetById;
using MultiTenantEcommerce.Application.Sales.Orders.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<ActionResult<List<OrderResponseDTO>>> GetOrders([FromQuery] GetFilteredOrdersQuery filter)
    {
        var orders = await _mediator.Send(filter);

        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponseWithPayment>> GetById(Guid id)
    {
        var query = new GetOrderByIdQuery(id);

        var order = await _mediator.Send(query);

        return Ok(order);
    }


    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<OrderResponseDTO>> ChangeOrderStatus(Guid id, [FromBody] ChangeOrderStatusDTO statusDTO)
    {
        var command = new ChangeOrderStatusCommand(id, statusDTO.Status);

        var order = await _mediator.Send(command);

        return Ok(order);
    }


    [HttpGet("customer")]
    public async Task<ActionResult<List<OrderResponseDTO>>> GetByCustomerId(Guid id)
    {
        var command = new GetOrderByCustomerIdQuery(id);

        var orders = await _mediator.Send(command);

        return Ok(orders);
    }
}
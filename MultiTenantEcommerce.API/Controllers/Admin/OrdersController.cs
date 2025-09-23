using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Sales.Orders.Commands.ChangeStatus;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Sales.Orders.Queries.GetByCustomerId;
using MultiTenantEcommerce.Application.Sales.Orders.Queries.GetById;
using MultiTenantEcommerce.Application.Sales.Orders.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "EmployeeOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.order")]
    [HttpGet()]
    public async Task<ActionResult<List<OrderResponseDTO>>> GetOrders([FromQuery] GetFilteredOrdersQuery filter)
    {
        var orders = await _mediator.Send(filter);

        return Ok(orders);
    }

    [HasPermission("read.order")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponseWithPayment>> GetOrderById(Guid id)
    {
        var query = new GetOrderByIdQuery(null, id);

        var order = await _mediator.Send(query);

        return Ok(order);
    }

    [HasPermission("read.order")]
    [HttpGet("customer/{id:guid}")]
    public async Task<ActionResult<List<OrderResponseDTO>>> GetOrdersByCustomerId(Guid id)
    {
        var command = new GetOrdersByCustomerIdQuery(id);

        var orders = await _mediator.Send(command);

        return Ok(orders);
    }

    [HasPermission("update.order")]
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<OrderResponseDTO>> ChangeOrderStatus(Guid id, [FromBody] ChangeOrderStatusDTO statusDTO)
    {
        var command = new ChangeOrderStatusCommand(id, statusDTO.Status);

        var order = await _mediator.Send(command);

        return Ok(order);
    }
}

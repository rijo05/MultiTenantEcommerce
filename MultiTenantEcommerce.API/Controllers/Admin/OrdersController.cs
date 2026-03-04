using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetFilteredAdmin;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "TenantMemberOnly")]
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
    [HttpGet]
    public async Task<ActionResult<List<OrderResponseDTO>>> GetOrders([FromQuery] GetFilteredOrdersAdminQuery filter)
    {
        var orders = await _mediator.Send(filter);

        return Ok(orders);
    }

    [HasPermission("read.order")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponseDetailDTO>> GetOrderById(Guid id)
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
    public async Task<ActionResult<OrderResponseDetailDTO>> ChangeOrderStatus(Guid id,
        [FromBody] ChangeOrderStatusDTO statusDTO)
    {
        var command = new ChangeOrderStatusCommand(id, statusDTO.Status);

        var order = await _mediator.Send(command);

        return Ok(order);
    }
}
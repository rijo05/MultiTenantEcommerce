using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.DTOs.Order;
using MultiTenantEcommerce.Application.DTOs.OrderItems;
using MultiTenantEcommerce.Application.Interfaces;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    #region GETs

    [HttpGet]
    public async Task<ActionResult<List<OrderResponseDTO>>> GetOrders([FromQuery] OrderFilterDTO filter)
    {
        var orders = await _orderService.GetFilteredOrdersAsync(filter);
        return Ok(orders);
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<OrderResponseDTO>> GetById(Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);

        if (order is null)
            return NotFound($"Order with ID '{id}' not found.");

        return Ok(order);
    }

    [HttpGet("{id:Guid}/items")]
    public async Task<ActionResult<List<OrderItemResponseDTO>>> GetOrderItems(Guid id)
    {
        var orderItems = await _orderService.GetOrderItemsAsync(id);

        return Ok(orderItems);
    }


    #endregion


    #region CHANGE ORDER STATUS


    [HttpPost("{id:guid}/status")]
    public async Task<ActionResult<OrderResponseDTO>> ChangeOrderStatus(Guid id, [FromBody] ChangeOrderStatusDTO statusDTO)
    {
        var order = await _orderService.ChangeOrderStatus(id, statusDTO);

        return Ok(order);
    }
    #endregion


    #region CREATE ORDER

    [HttpPost]
    public async Task<ActionResult<OrderResponseDTO>> CreateOrder([FromBody] CreateOrderDTO orderDTO)
    {
        if (orderDTO is null)
            return BadRequest("Order data must be provided.");

        var order = await _orderService.CreateOrderAsync(orderDTO);

        return CreatedAtAction(
            nameof(GetById),
            new { id = order.Id },
            order
            );
    }

    #endregion
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Inventory.Commands.DecrementStock;
using MultiTenantEcommerce.Application.Inventory.Commands.IncrementStock;
using MultiTenantEcommerce.Application.Inventory.Commands.SetMinimumStockLevel;
using MultiTenantEcommerce.Application.Inventory.Commands.SetStock;
using MultiTenantEcommerce.Application.Inventory.DTOs;
using MultiTenantEcommerce.Application.Inventory.Queries.GetByProductId;
using MultiTenantEcommerce.Application.Inventory.Queries.GetStockMovements;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "EmployeeOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.stock")]
    [HttpGet("{productId:guid}")]
    [ProducesResponseType(typeof(StockResponseAdminDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<IStockDTO>> GetStockByProductId(Guid productId)
    {
        var query = new GetStockByProductIdQuery(productId, true);

        var stock = await _mediator.Send(query);

        return Ok(stock);
    }

    [HasPermission("update.stock")]
    [HttpPatch("{productId:guid}/increment")]
    public async Task<ActionResult<StockResponseAdminDTO>> IncrementStock(Guid productId, [FromBody] int Quantity)
    {
        var command = new IncrementStockCommand(productId, Quantity);
        var stock = await _mediator.Send(command);

        return Ok(stock);
    }

    [HasPermission("update.stock")]
    [HttpPatch("{productId:guid}/decrement")]
    public async Task<ActionResult<StockResponseAdminDTO>> DecrementStock(Guid productId, [FromBody] int Quantity)
    {
        var command = new DecrementStockCommand(productId, Quantity);
        var stock = await _mediator.Send(command);

        return Ok(stock);
    }

    [HasPermission("update.stock")]
    [HttpPatch("{productId:guid}/minimum-level")]
    public async Task<ActionResult<StockResponseAdminDTO>> SetMinimumStockLevel(Guid productId, [FromBody] int Quantity)
    {
        var command = new SetMinimumStockLevelCommand(productId, Quantity);

        var stock = await _mediator.Send(command);

        return Ok(stock);
    }

    [HasPermission("update.stock")]
    [HttpPut("{productId:guid}")]
    public async Task<ActionResult<StockResponseAdminDTO>> SetStock(Guid productId, [FromBody] int Quantity)
    {
        var command = new SetStockCommand(productId, Quantity);
        var stock = await _mediator.Send(command);

        return Ok(stock);
    }

    [HasPermission("read.stock")]
    [HttpGet("movements")]
    public async Task<ActionResult<List<StockMovementResponseDTO>>> GetStockMovements([FromQuery] GetFilteredStockMovementsQuery query)
    {
        var movements = await _mediator.Send(query);

        return Ok(movements);
    }
}

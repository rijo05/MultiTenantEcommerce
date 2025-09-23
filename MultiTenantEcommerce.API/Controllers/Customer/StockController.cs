using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Inventory.DTOs;
using MultiTenantEcommerce.Application.Inventory.Queries.GetByProductId;

namespace MultiTenantEcommerce.API.Controllers.Customer;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{productId:guid}")]
    public async Task<ActionResult<StockResponseDTO>> GetStockByProductId(Guid productId)
    {
        var query = new GetStockByProductIdQuery(productId, false);

        var stock = await _mediator.Send(query);

        return Ok(stock);
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Payment.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.AddItem;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Checkout;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Clear;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Create;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.RemoveItem;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Queries.GetByCustomerId;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Queries.GetById;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost("checkout")]
    public async Task<ActionResult<PaymentResultDTO>> Checkout([FromBody] CheckoutCommand command)
    {
        var paymentResult = await _mediator.Send(command);
        return Ok(paymentResult);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CartResponseDTO>> GetById(Guid id)
    {
        var query = new GetCartByIdQuery(id);
        var cart = await _mediator.Send(query);

        return Ok(cart);
    }

    [HttpGet("customer/{id:guid}")]
    public async Task<ActionResult<CartResponseDTO>> GetByCustomerId(Guid id)
    {
        var query = new GetCartByCustomerIdQuery(id);
        var cart = await _mediator.Send(query);

        return Ok(cart);
    }

    [HttpPost("createcart")]
    public async Task<ActionResult<CartResponseDTO>> CreateCart(CreateCartCommand command)
    {
        if (command is null)
            return BadRequest("Cart data must be provided.");

        var cart = await _mediator.Send(command);

        return Ok(cart);
    }

    [HttpPost("{cartId:guid}/items")]
    public async Task<ActionResult<CartResponseDTO>> AddItem(Guid cartId, [FromBody] ModifyQuantityDTO quantityDTO)
    {
        var command = new AddCartItemCommand(cartId, quantityDTO.ProductId, quantityDTO.Quantity);
        var cart = await _mediator.Send(command);

        return Ok(cart);
    }

    [HttpDelete("{cartId:guid}/items/{productId:guid}")]
    public async Task<ActionResult<CartResponseDTO>> RemoveItem(Guid cartId, Guid productId)
    {
        var command = new RemoveCartItemCommand(cartId, productId);

        var cart = await _mediator.Send(command);

        return Ok(cart);
    }

    [HttpPatch("{cartId:guid}/items/{productId:guid}")]
    public async Task<ActionResult<CartResponseDTO>> DecreaseItem(Guid cartId, Guid productId, [FromBody] QuantityDTO quantityDTO)
    {
        var command = new DecreaseCartItemCommand(cartId, productId, quantityDTO.Quantity);

        var cart = await _mediator.Send(command);

        return Ok(cart);
    }


    [HttpDelete("{cartId}/items")]
    public async Task<ActionResult<CartResponseDTO>> ClearCart(Guid cartId)
    {
        var command = new ClearCartCommand(cartId);

        var cart = await _mediator.Send(command);

        return Ok(cart);
    }
}

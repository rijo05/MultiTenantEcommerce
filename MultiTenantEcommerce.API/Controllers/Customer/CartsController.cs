using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Extensions;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.AddItem;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Checkout;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Clear;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Create;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.RemoveItem;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.Queries.GetById;

namespace MultiTenantEcommerce.API.Controllers.Customer;

[ApiController]
[Authorize(Policy = "CustomerOnly")]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<PaymentResultDTO>> Checkout([FromBody] CheckoutDTO checkout)
    {
        var userId = User.GetUserId();
        var command = new CheckoutCommand(userId, checkout.AddressDTO, checkout.PaymentMethod);

        var paymentResult = await _mediator.Send(command);
        return Ok(paymentResult);
    }

    [HttpGet("me")]
    public async Task<ActionResult<CartResponseDTO>> GetMyCart()
    {
        var userId = User.GetUserId();
        var query = new GetCartByIdQuery(userId);

        var cart = await _mediator.Send(query);

        return Ok(cart);
    }

    [HttpPost("createcart")]
    public async Task<ActionResult<CartResponseDTO>> CreateCart()
    {
        var userId = User.GetUserId();
        var command = new CreateCartCommand(userId);

        var cart = await _mediator.Send(command);

        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<ActionResult<CartResponseDTO>> AddItem([FromBody] ModifyQuantityDTO quantityDTO)
    {
        var userId = User.GetUserId();
        var command = new AddCartItemCommand(userId, quantityDTO.ProductId, quantityDTO.Quantity);

        var cart = await _mediator.Send(command);

        return Ok(cart);
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<ActionResult<CartResponseDTO>> RemoveItem(Guid productId)
    {
        var userId = User.GetUserId();
        var command = new RemoveCartItemCommand(userId, productId);

        var cart = await _mediator.Send(command);

        return Ok(cart);
    }

    [HttpPatch("items/{productId:guid}")]
    public async Task<ActionResult<CartResponseDTO>> DecreaseItem(Guid productId, [FromBody] QuantityDTO quantityDTO)
    {
        var userId = User.GetUserId();
        var command = new DecreaseCartItemCommand(userId, productId, quantityDTO.Quantity);

        var cart = await _mediator.Send(command);

        return Ok(cart);
    }


    [HttpDelete()]
    public async Task<ActionResult<CartResponseDTO>> ClearCart()
    {
        var userId = User.GetUserId();

        var command = new ClearCartCommand(userId);

        var cart = await _mediator.Send(command);

        return Ok(cart);
    }
}

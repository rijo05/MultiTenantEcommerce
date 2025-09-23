using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Extensions;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetByCustomerId;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetById;

namespace MultiTenantEcommerce.API.Controllers.Customer;

[ApiController]
[Authorize(Policy = "CustomerOnly")]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("me")]
    public async Task<ActionResult<List<OrderPaymentResponseDTO>>> GetMyPayments()
    {
        var userId = User.GetUserId();

        var query = new GetOrderPaymentsByCustomerIdQuery(userId);

        var payments = await _mediator.Send(query);

        return Ok(payments);
    }

    [HttpGet("me/{id:guid}")]
    public async Task<ActionResult<OrderPaymentResponseDTO>> GetById(Guid id)
    {
        var query = new GetOrderPaymentByIdQuery(id, User.GetUserId());

        var payment = await _mediator.Send(query);

        return Ok(payment);
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.API.Authorization;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetByCustomerId;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetById;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetFiltered;

namespace MultiTenantEcommerce.API.Controllers.Admin;

[ApiController]
[Authorize(Policy = "EmployeeOnly")]
[Area("Admin")]
[Route("api/[area]/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HasPermission("read.payment")]
    [HttpGet]
    public async Task<ActionResult<List<OrderPaymentResponseDTO>>> GetOrderPayments([FromQuery] GetFilteredOrderPaymentsQuery orderPaymentsQuery)
    {
        var orders = await _mediator.Send(orderPaymentsQuery);

        return Ok(orders);
    }

    [HasPermission("read.payment")]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderPaymentResponseDTO>> GetById(Guid id)
    {
        var query = new GetOrderPaymentByIdQuery(null, id);

        var payment = await _mediator.Send(query);

        return Ok(payment);
    }

    [HasPermission("read.payment")]
    [HttpGet("customer/{id:guid}")]
    public async Task<ActionResult<List<OrderPaymentResponseDTO>>> GetByCustomerId(Guid id)
    {
        var query = new GetOrderPaymentsByCustomerIdQuery(id);

        var payments = await _mediator.Send(query);

        return Ok(payments);
    }
}

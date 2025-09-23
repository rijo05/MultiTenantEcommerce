using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetById;
public record GetOrderPaymentByIdQuery(
    Guid? CustomerId,
    Guid PaymentId) : IQuery<OrderPaymentResponseDTO>;

using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.DTOs;

namespace MultiTenantEcommerce.Application.Payment.Queries.GetById;
public record GetPaymentByIdQuery(
    Guid PaymentId) : IQuery<PaymentResponseDTO>;

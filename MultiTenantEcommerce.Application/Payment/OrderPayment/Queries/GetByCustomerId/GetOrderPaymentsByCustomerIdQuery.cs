using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetByCustomerId;
public record GetOrderPaymentsByCustomerIdQuery(
    Guid customerId,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<OrderPaymentResponseDTO>>;

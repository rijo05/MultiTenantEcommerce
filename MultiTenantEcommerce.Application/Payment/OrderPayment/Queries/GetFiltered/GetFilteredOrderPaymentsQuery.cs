using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetFiltered;
public record GetFilteredOrderPaymentsQuery(
    Guid? CustomerId = null,
    Guid? OrderId = null,
    PaymentStatus? Status = null,
    PaymentMethod? Method = null,
    DateTime? MinCreatedAt = null,
    DateTime? MaxCreatedAt = null,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<OrderPaymentResponseDTO>>;

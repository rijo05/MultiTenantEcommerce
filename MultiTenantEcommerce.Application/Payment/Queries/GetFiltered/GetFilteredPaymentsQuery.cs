using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Payment.Queries.GetFiltered;
public record GetFilteredPaymentsQuery(
    Guid? PayerId = null,
    Guid? PayeeId = null,
    PaymentStatus? Status = null,
    PaymentReason? Reason = null,
    Guid? ReasonId = null,
    DateTime? MinDate = null,
    DateTime? MaxDate = null,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IQuery<List<PaymentResponseDTO>>;

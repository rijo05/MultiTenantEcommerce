using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetByIdAdmin;
public record PaymentAttemptAdminDTO(
    Guid PaymentId,
    DateTime CreatedAt,
    string Status,
    string? StripeSessionId,
    string? TransactionId,
    string? FailureReason,
    DateTime? CompletedAt);

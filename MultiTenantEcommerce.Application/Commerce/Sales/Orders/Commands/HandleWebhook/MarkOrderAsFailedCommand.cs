using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.HandleWebhook;

public record MarkOrderAsFailedCommand(
    Guid OrderId,
    Guid PaymentId,
    string FailureReason) : ICommand<Unit>;
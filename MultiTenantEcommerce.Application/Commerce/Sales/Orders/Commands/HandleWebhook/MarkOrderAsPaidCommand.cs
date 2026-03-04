using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.HandleWebhook;

public record MarkOrderAsPaidCommand(
    Guid OrderId,
    Guid PaymentId,
    string TransactionId) : ICommand<Unit>;
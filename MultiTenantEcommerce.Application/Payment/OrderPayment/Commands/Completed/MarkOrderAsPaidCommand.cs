using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Commands.Completed;
public record MarkOrderAsPaidCommand(
    Guid OrderId,
    string TransactionId) : ICommand<Unit>;

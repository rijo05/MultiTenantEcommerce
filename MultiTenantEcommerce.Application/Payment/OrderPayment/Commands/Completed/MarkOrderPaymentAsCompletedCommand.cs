using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Commands.Completed;
public record MarkOrderPaymentAsCompletedCommand(
    Guid PaymentId,
    string TransactionId) : ICommand<Unit>;

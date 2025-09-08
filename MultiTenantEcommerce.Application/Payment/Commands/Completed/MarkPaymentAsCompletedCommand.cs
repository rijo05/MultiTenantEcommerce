using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Payment.Commands.StripeCompleted;
public record MarkPaymentAsCompletedCommand(
    Guid PaymentId,
    string TransactionId) : ICommand<Unit>;

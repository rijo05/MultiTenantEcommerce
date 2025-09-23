using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Commands.Failed;
public record MarkPaymentAsFailedCommand(
    Guid PaymentId,
    string? FailureReason) : ICommand<Unit>;


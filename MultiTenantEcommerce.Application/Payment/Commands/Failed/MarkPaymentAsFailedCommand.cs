using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Payment.Commands.StripeFailed;
public record MarkPaymentAsFailedCommand(
    Guid PaymentId,
    string? FailureReason) : ICommand<Unit>;


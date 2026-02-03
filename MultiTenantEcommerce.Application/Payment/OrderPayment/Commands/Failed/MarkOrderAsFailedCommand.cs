using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Commands.Failed;
public record MarkOrderAsFailedCommand(
    Guid OrderId) : ICommand<Unit>;


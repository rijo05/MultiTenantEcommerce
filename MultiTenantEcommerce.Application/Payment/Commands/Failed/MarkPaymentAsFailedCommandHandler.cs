using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Payment.Interfaces;

namespace MultiTenantEcommerce.Application.Payment.Commands.StripeFailed;
public class MarkPaymentAsFailedCommandHandler : ICommandHandler<MarkPaymentAsFailedCommand, Unit>
{
    private readonly IPaymentRepository _repository;

    public MarkPaymentAsFailedCommandHandler(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(MarkPaymentAsFailedCommand request, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(request.PaymentId)
            ?? throw new Exception("Payment not found");

        payment.MarkAsFailed(request.FailureReason);
        await _repository.UpdateAsync(payment);

        return Unit.Value;
    }
}

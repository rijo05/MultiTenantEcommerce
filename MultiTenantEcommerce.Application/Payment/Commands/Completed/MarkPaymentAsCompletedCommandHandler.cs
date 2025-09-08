using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Payment.Interfaces;

namespace MultiTenantEcommerce.Application.Payment.Commands.StripeCompleted;
public class MarkPaymentAsCompletedCommandHandler : ICommandHandler<MarkPaymentAsCompletedCommand, Unit>
{
    private readonly IPaymentRepository _repository;

    public MarkPaymentAsCompletedCommandHandler(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(MarkPaymentAsCompletedCommand request, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(request.PaymentId)
            ?? throw new Exception("Payment not found");

        payment.MarkAsCompleted(request.TransactionId);
        await _repository.UpdateAsync(payment);

        return Unit.Value;
    }
}

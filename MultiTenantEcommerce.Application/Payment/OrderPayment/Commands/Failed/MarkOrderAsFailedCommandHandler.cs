using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Commands.Failed;
public class MarkOrderAsFailedCommandHandler : ICommandHandler<MarkOrderAsFailedCommand, Unit>
{
    private readonly IOrderPaymentRepository _orderPaymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkOrderAsFailedCommandHandler(IOrderPaymentRepository orderPaymentRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderPaymentRepository = orderPaymentRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(MarkOrderAsFailedCommand request, CancellationToken cancellationToken)
    {
        //var payment = await _orderPaymentRepository.GetByIdAsync(request.PaymentId)
        //    ?? throw new Exception("Payment not found");

        //var order = await _orderRepository.GetByIdAsync(payment.OrderId)
        //    ?? throw new Exception("Order couldnt be foound, this shouldnt happen");

        //payment.MarkAsFailed(request.FailureReason);
        //order.MarkAsFailed();

        //await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}

using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.HandleWebhook;

public class MarkOrderAsFailedCommandHandler : ICommandHandler<MarkOrderAsFailedCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkOrderAsFailedCommandHandler(IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(MarkOrderAsFailedCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order == null)
        {
            //provavelmente deve ter falhado logo no checkout 
            return Unit.Value;
        }

        order.RegisterFailedPaymentAttempt(request.PaymentId ,request.FailureReason);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
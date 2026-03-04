using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.HandleWebhook;

public class MarkOrderAsPaidCommandHandler : ICommandHandler<MarkOrderAsPaidCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkOrderAsPaidCommandHandler(IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(MarkOrderAsPaidCommand request, CancellationToken ct)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order == null)
            return Unit.Value;

        order.MarkAsPaid(request.PaymentId ,request.TransactionId);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
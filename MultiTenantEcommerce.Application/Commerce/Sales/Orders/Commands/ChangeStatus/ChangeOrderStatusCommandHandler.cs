using MediatR;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Enums;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.Payment.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.ChangeStatus;

public class ChangeOrderStatusCommandHandler : ICommandHandler<ChangeOrderStatusCommand, Unit>
{
    private readonly IOrderPaymentRepository _orderPaymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeOrderStatusCommandHandler(IOrderRepository orderRepository,
        IOrderPaymentRepository orderPaymentRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _orderPaymentRepository = orderPaymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(ChangeOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId)
                    ?? throw new Exception("Order doesnt exist");

        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var parsedStatus))
            throw new ArgumentException($"Invalid Status: {request.Status}");

        order.ChangeStatus(parsedStatus);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
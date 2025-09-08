using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Sales.Orders.Mappers;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.Orders.Commands.ChangeStatus;
public class ChangeOrderStatusCommandHandler : ICommandHandler<ChangeOrderStatusCommand, OrderResponseDTO>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly OrderMapper _orderMapper;

    public ChangeOrderStatusCommandHandler(IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        OrderMapper orderMapper)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _orderMapper = orderMapper;
    }

    public async Task<OrderResponseDTO> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.orderId)
            ?? throw new Exception("Order doesnt exist");

        order.ChangeStatus(request.Status);

        await _unitOfWork.CommitAsync();

        return _orderMapper.ToOrderResponseDTO(order);
    }
}

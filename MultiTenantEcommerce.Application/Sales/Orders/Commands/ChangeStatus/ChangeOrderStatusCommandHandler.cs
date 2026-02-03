using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Sales.Orders.Mappers;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Shipping.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.Orders.Commands.ChangeStatus;
public class ChangeOrderStatusCommandHandler : ICommandHandler<ChangeOrderStatusCommand, OrderResponseDetailDTO>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IShipmentRepository _shipmentRepository;
    private readonly IOrderPaymentRepository _orderPaymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly OrderMapper _orderMapper;

    public ChangeOrderStatusCommandHandler(IOrderRepository orderRepository,
        IShipmentRepository shipmentRepository,
        IOrderPaymentRepository orderPaymentRepository,
        IUnitOfWork unitOfWork,
        OrderMapper orderMapper)
    {
        _orderRepository = orderRepository;
        _shipmentRepository = shipmentRepository;
        _orderPaymentRepository = orderPaymentRepository;
        _unitOfWork = unitOfWork;
        _orderMapper = orderMapper;
    }

    public async Task<OrderResponseDetailDTO> Handle(ChangeOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.orderId)
            ?? throw new Exception("Order doesnt exist");

        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var parsedStatus))
            throw new ArgumentException($"Invalid Status: {request.Status}");

        order.ChangeStatus(parsedStatus);

        await _unitOfWork.CommitAsync();

        var shipment = await _shipmentRepository.GetByOrderId(order.Id);
        var payment = await _orderPaymentRepository.GetByOrderId(order.Id);

        return _orderMapper.ToOrderResponseDetailDTO(order, payment, shipment);
    }
}

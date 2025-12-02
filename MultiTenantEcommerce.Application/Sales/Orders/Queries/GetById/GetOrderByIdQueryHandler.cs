using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Sales.Orders.Mappers;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Shipping.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.Orders.Queries.GetById;
public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderResponseDetailDTO>
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderMapper _orderMapper;
    private readonly IOrderPaymentRepository _paymentRepository;
    private readonly IShipmentRepository _shipmentRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository,
        OrderMapper orderMapper,
        IOrderPaymentRepository paymentRepository,
        IShipmentRepository shipmentRepository)
    {
        _orderRepository = orderRepository;
        _orderMapper = orderMapper;
        _paymentRepository = paymentRepository;
        _shipmentRepository = shipmentRepository;
    }

    public async Task<OrderResponseDetailDTO> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId)
            ?? throw new Exception("Order doesnt exist");

        var payment = await _paymentRepository.GetByOrderId(request.OrderId);

        var shipment = await _shipmentRepository.GetByOrderId(request.OrderId);

        if (request.customerId != null && order.CustomerId != request.customerId)
            throw new Exception("This order doesnt belong to you");

        return _orderMapper.ToOrderResponseDetailDTO(order, payment, shipment);
    }
}

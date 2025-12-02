using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Shipping.Interfaces;

namespace MultiTenantEcommerce.Application.Shipping.EventHandlers;
public class CreateShipmentOnOrderPaidEventHandler : IEventHandler<OrderPaidEvent>
{
    private readonly IShippingService _shippingService;
    private readonly IShipmentRepository _shipmentRepository;
    private readonly IOrderRepository _orderRepository;

    public CreateShipmentOnOrderPaidEventHandler(IShippingService shippingService,
        IShipmentRepository shipmentRepository,
        IOrderRepository orderRepository)
    {
        _shippingService = shippingService;
        _orderRepository = orderRepository;
        _shipmentRepository = shipmentRepository;
    }

    public async Task HandleAsync(OrderPaidEvent domainEvent)
    {
        var order = await _orderRepository.GetByIdAsync(domainEvent.OrderId)
            ?? throw new Exception("Order doesnt exist");

        var shipment = await _shippingService.CreateShipment(domainEvent.TenantId,
            order,
            order.ShipmentCarrier);

        await _shipmentRepository.AddAsync(shipment);
    }
}

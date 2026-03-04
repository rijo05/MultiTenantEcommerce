using MultiTenantEcommerce.Application.Commerce.Shipping.Services;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Interfaces;
using MultiTenantEcommerce.Shared.Application;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.EventHandlers;

public class CreateShipmentOnOrderPaidEventHandler : IEventHandler<OrderPaidEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IShipmentRepository _shipmentRepository;
    private readonly IShippingService _shippingService;

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
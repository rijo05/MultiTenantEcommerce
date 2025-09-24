using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;

namespace MultiTenantEcommerce.Application.Inventory.EventHandlers;
public class ReleaseStockOnOrderPaymentFailedEventHandler : IEventHandler<OrderPaymentFailedEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IStockService _stockService;

    public ReleaseStockOnOrderPaymentFailedEventHandler(IOrderRepository orderRepository,
        IStockService stockService)
    {
        _orderRepository = orderRepository;
        _stockService = stockService;
    }

    public async Task HandleAsync(OrderPaymentFailedEvent domainEvent)
    {
        var order = await _orderRepository.GetByIdAsync(domainEvent.OrderId)
            ?? throw new Exception("Order not found");

        await _stockService.ReleaseReservedStock(order);
    }
}

using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;

namespace MultiTenantEcommerce.Application.Inventory.EventHandlers;
public class CommitStockOnOrderPaidEventHandler : IEventHandler<OrderPaidEvent>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IStockService _stockService;

    public CommitStockOnOrderPaidEventHandler(IOrderRepository orderRepository,
        IStockService stockService)
    {
        _orderRepository = orderRepository;
        _stockService = stockService;
    }   

    public async Task HandleAsync(OrderPaidEvent domainEvent)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(domainEvent.OrderId)
            ?? throw new Exception("Order not found");

        await _stockService.CommitStock(order);
    }
}

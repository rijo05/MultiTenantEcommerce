using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.EventHandlers;

public class CommitStockOnOrderPaidEventHandler : IAsyncHandler<OrderPaidEvent>
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
        var order = await _orderRepository.GetByIdAsync(domainEvent.OrderId)
                    ?? throw new Exception("Order not found");

        await _stockService.CommitStock(order);
    }
}
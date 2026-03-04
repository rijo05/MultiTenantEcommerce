using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Enums;

namespace MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Validator;

public static class OrderStatusTransitionValidator
{
    private static readonly Dictionary<OrderStatus, List<OrderStatus>> AllowedTransitions = new()
    {
        {
            OrderStatus.PendingPayment,
            new List<OrderStatus> { OrderStatus.Cancelled, OrderStatus.Processing }
        },
        { OrderStatus.Processing, new List<OrderStatus> { OrderStatus.Cancelled, OrderStatus.Completed } },
        { OrderStatus.Cancelled, new List<OrderStatus>() },
        { OrderStatus.Refunded, new List<OrderStatus>() },
        { OrderStatus.Completed, new List<OrderStatus>() {OrderStatus.Refunded } }
    };

    public static void ValidateTransition(OrderStatus CurrentStatus, OrderStatus newStatus)
    {
        if (AllowedTransitions.TryGetValue(CurrentStatus, out var allowed) && allowed.Contains(newStatus)) return;

        throw new Exception($"Invalid order status transition from {CurrentStatus} to {newStatus}.");
    }
}
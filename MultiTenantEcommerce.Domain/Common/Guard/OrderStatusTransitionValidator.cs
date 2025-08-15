using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Common.Guard;

public static class OrderStatusTransitionValidator
{
    private static readonly Dictionary<OrderStatus, List<OrderStatus>> AllowedTransitions = new()
    {
        {OrderStatus.PendingPayment, new List<OrderStatus>() {OrderStatus.Cancelled, OrderStatus.Processing } },
        {OrderStatus.Processing, new List<OrderStatus>() {OrderStatus.Cancelled, OrderStatus.Shipped } },
        {OrderStatus.Shipped, new List<OrderStatus>() {OrderStatus.Delivered } },
        {OrderStatus.Delivered, new List<OrderStatus>() {OrderStatus.Invoiced } },
        {OrderStatus.Invoiced, new List<OrderStatus>() },
        {OrderStatus.Cancelled, new List<OrderStatus>() }
    };

    public static void ValidateTransition(OrderStatus CurrentStatus, OrderStatus newStatus)
    {
        if (AllowedTransitions.TryGetValue(CurrentStatus, out var allowed) && allowed.Contains(newStatus))
        {
            return;
        }

        throw new Exception($"Invalid order status transition from {CurrentStatus} to {newStatus}.");
    }
}

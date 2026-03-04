namespace MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Enums;

public enum OrderStatus
{
    PendingPayment,
    Processing,
    Completed,
    Cancelled,
    Refunded
}
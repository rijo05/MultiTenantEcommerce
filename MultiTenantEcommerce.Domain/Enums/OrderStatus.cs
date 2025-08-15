namespace MultiTenantEcommerce.Domain.Enums;
public enum OrderStatus
{
    PendingPayment,
    Processing,
    Shipped,
    Delivered,
    Invoiced,
    Cancelled
}

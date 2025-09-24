using Microsoft.AspNetCore.Routing;

namespace MultiTenantEcommerce.Application.Common.Helpers.EmailKeys;
public static class EmailMetadataKeys
{
    public const string CustomerName = "CustomerName";
    public const string CustomerEmail = "CustomerEmail";


    public const string EmployeeName = "EmployeeName";
    public const string RolesHtml = "RolesHtml";
    public const string TenantName = "TenantName";
    public const string ChangePasswordLink = "ChangePasswordLink";


    public const string ItemsHtml = "ItemsHtml";
    public const string OrderId = "OrderId";
    public const string AmountPaid = "AmountPaid";
    public const string PaymentMethod = "PaymentMethod";
    public const string BillingAddress = "BillingAddress";
    public const string TrackingNumber = "TrackingNumber";
    public const string TrackingLink = "TrackingLink";
    public const string DeliveryDate = "DeliveryDate";
    public const string FailureReason = "FailureReason";
    public const string CarrierName = "CarrierName";


    public const string ProductId = "ProductId";
    public const string ProductName = "ProductName";
    public const string CurrentQuantity = "CurrentQuantity";
    public const string MinimumQuantity = "MinimumQuantity";
}

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrderById;
public record PaymentSummaryCustomerDTO(
    string Status,
    string? FailureReason);

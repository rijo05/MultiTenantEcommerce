namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetCheckoutSummary;
public record CheckoutSummaryDTO(
    decimal SubTotal,
    decimal ShippingCost,
    decimal TotalPrice,
    bool IsStockAvailable,
    List<UnavailableItemDTO> UnavailableItems);

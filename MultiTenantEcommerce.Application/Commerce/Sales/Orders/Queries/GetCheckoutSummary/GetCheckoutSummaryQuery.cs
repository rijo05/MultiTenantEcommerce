using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetCheckoutSummary;
public record GetCheckoutSummaryQuery(
    Guid CustomerId,
    Guid ShippingAddressId,
    ShipmentCarrier Carrier) : IQuery<CheckoutSummaryDTO>;

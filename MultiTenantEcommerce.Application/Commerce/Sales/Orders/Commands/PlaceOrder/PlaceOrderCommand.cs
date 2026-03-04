using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrderById;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.PlaceOrder;
public record PlaceOrderCommand(
    Guid CustomerId,
    Guid AddressId,
    ShipmentCarrier Carrier) : ICommand<OrderDetailDTO>;
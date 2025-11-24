using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Shipping.Enums;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Checkout;
public record CheckoutCommand(
    Guid CustomerId,
    CreateAddressDTO Address,
    PaymentMethod PaymentMethod,
    ShipmentCarrier Carrier) : ICommand<PaymentResultDTO>;

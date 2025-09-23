using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Checkout;
public record CheckoutCommand(
    Guid CustomerId,
    CreateAddressDTO Address,
    PaymentMethod PaymentMethod) : ICommand<PaymentResultDTO>;

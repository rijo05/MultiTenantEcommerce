using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.DTOs;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Checkout;
public record CheckoutCommand(
    Guid customerId,
    CreateAddressDTO Address,
    PaymentMethod PaymentMethod) : ICommand<PaymentResultDTO>;

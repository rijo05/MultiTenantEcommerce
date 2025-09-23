using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
public record CheckoutDTO(
    CreateAddressDTO AddressDTO,
    PaymentMethod PaymentMethod);

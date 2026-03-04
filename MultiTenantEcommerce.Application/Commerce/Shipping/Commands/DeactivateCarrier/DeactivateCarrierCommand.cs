using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.Commands.DeactivateCarrier;

public record DeactivateCarrierCommand(
    ShipmentCarrier Carrier) : ICommand<Unit>;
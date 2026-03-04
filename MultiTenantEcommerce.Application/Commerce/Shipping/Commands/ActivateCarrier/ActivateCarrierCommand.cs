using MediatR;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.Commands.ActivateCarrier;

public record ActivateCarrierCommand(
    ShipmentCarrier Carrier) : ICommand<Unit>;
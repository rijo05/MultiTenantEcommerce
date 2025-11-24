using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Shipping.Enums;

namespace MultiTenantEcommerce.Application.Shipping.Commands.DeactivateCarrier;
public record DeactivateCarrierCommand(
    ShipmentCarrier Carrier) : ICommand<Unit>;

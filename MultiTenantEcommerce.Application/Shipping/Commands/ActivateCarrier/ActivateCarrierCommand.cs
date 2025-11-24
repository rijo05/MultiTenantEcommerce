using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Shipping.Enums;

namespace MultiTenantEcommerce.Application.Shipping.Commands.ActivateCarrier;
public record ActivateCarrierCommand(
    ShipmentCarrier Carrier) : ICommand<Unit>;

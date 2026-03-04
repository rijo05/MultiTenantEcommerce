using MediatR;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.DTOs;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.ChangeStatus;

public record ChangeOrderStatusCommand(
    Guid OrderId,
    string Status) : ICommand<Unit>;
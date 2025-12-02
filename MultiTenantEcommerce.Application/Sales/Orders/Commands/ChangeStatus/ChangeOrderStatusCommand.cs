using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;

namespace MultiTenantEcommerce.Application.Sales.Orders.Commands.ChangeStatus;
public record ChangeOrderStatusCommand(
    Guid orderId,
    string Status) : ICommand<OrderResponseDetailDTO>;

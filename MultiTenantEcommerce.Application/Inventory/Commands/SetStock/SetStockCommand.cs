using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Inventory.DTOs;

namespace MultiTenantEcommerce.Application.Inventory.Commands.SetStock;
public record SetStockCommand(
    Guid ProductId,
    int Quantity) : ICommand<StockResponseAdminDTO>;

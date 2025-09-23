using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Inventory.DTOs;

namespace MultiTenantEcommerce.Application.Inventory.Commands.SetMinimumStockLevel;
public record SetMinimumStockLevelCommand(
    Guid ProductId,
    int Quantity) : ICommand<StockResponseAdminDTO>;

using MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.Commands.SetMinimumStockLevel;

public record SetMinimumStockLevelCommand(
    Guid ProductId,
    int Quantity) : ICommand<StockResponseAdminDTO>;
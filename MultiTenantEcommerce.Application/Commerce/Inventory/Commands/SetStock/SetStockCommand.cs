using MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.Commands.SetStock;

public record SetStockCommand(
    Guid ProductId,
    int Quantity) : ICommand<StockResponseAdminDTO>;
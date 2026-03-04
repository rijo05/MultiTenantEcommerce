using MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.Commands.DecrementStock;

public record DecrementStockCommand(
    Guid ProductId,
    int Quantity) : ICommand<StockResponseAdminDTO>;
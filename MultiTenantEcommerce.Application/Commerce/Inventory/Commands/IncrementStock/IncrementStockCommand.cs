using MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.Commands.IncrementStock;

public record IncrementStockCommand(
    Guid ProductId,
    int Quantity) : ICommand<StockResponseAdminDTO>;
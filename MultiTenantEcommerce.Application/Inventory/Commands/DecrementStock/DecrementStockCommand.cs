using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Inventory.DTOs;

namespace MultiTenantEcommerce.Application.Inventory.Commands.DecrementStock;
public record DecrementStockCommand(
    Guid ProductId,
    int Quantity) : ICommand<StockResponseAdminDTO>;

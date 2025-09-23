using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Inventory.DTOs;

namespace MultiTenantEcommerce.Application.Inventory.Commands.IncrementStock;
public record IncrementStockCommand(
    Guid ProductId,
    int Quantity) : ICommand<StockResponseAdminDTO>;

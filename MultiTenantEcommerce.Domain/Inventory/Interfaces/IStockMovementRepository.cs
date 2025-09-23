using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Inventory.Entities;

namespace MultiTenantEcommerce.Domain.Inventory.Interfaces;
public interface IStockMovementRepository : IRepository<StockMovement>
{
    public Task<List<StockMovement>> GetFilteredAsync(
    Guid? productId = null,
    int? minQuantity = null,
    int? maxQuantity = null,
    string? reason = null,
    DateTime? minDate = null,
    DateTime? maxDate = null,
    int page = 1,
    int pageSize = 20,
    SortOptions sort = SortOptions.TimeDesc);
}


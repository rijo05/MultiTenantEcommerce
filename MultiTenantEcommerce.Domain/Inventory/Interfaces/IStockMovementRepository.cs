using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Inventory.Entities;

namespace MultiTenantEcommerce.Domain.Inventory.Interfaces;
public interface IStockMovementRepository : IRepository<StockMovement>
{
    public Task<List<StockMovement>> GetFilteredAsync(
    Guid? productId = null,
    int? quantity = null,
    DateTime? minDate = null,
    DateTime? maxDate = null,
    string? reason = null,
    int page = 1,
    int pageSize = 20,
    SortOptions? sort = null);
}


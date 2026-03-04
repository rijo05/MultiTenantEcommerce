using MultiTenantEcommerce.Domain.Commerce.Inventory.Entities;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;

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
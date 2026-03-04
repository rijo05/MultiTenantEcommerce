using MultiTenantEcommerce.Application.Commerce.Inventory.Common.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.Queries.GetByProductId;

public record GetStockByProductIdQuery(
    Guid ProductId,
    bool IsAdmin) : IQuery<IStockDTO>;
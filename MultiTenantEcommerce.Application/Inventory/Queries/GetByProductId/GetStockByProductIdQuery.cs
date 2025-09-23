using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Inventory.DTOs;

namespace MultiTenantEcommerce.Application.Inventory.Queries.GetByProductId;
public record GetStockByProductIdQuery(
    Guid ProductId,
    bool IsAdmin) : IQuery<IStockDTO>;

using MediatR;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Tenants.Queries.Tenant.GetFiltered;

public record GetFilteredTenantsQuery(
    string? CompanyName,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IRequest<List<TenantResponseDTO>>;


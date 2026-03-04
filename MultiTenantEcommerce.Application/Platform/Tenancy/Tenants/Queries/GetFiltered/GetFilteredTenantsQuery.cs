using MediatR;
using MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Queries.GetFiltered;

public record GetFilteredTenantsQuery(
    string? CompanyName,
    int Page = 1,
    int PageSize = 20,
    SortOptions Sort = SortOptions.TimeDesc) : IRequest<List<TenantResponseDTO>>;
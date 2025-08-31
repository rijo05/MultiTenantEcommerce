using MediatR;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;

namespace MultiTenantEcommerce.Application.Tenants.Queries.Tenant.GetById;
public record GetTenantByIdQuery(
    Guid Id) : IRequest<TenantResponseDTO>;

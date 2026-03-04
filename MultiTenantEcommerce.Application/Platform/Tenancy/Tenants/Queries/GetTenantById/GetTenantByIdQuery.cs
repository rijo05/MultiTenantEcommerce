using MediatR;
using MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Queries.GetTenantById;

public record GetTenantByIdQuery(
    Guid Id) : IRequest<TenantResponseDTO>;
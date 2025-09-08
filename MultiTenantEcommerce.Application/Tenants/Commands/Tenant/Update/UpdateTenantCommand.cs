using MediatR;
using MultiTenantEcommerce.Application.Tenants.DTOs.Tenant;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Update;
public record UpdateTenantCommand(
    Guid Id,
    string CompanyName) : IRequest<TenantResponseDTO>;

using MediatR;
using MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Common.DTOs;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.UpdateSettings;

public record UpdateTenantCommand(
    Guid Id,
    string CompanyName) : IRequest<TenantResponseDTO>;
using MediatR;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Delete;
public record DeleteTenantCommand(
    Guid Id) : IRequest<Unit>;
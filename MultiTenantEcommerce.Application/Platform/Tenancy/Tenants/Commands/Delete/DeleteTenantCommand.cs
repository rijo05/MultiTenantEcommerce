using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.Delete;

public record DeleteTenantCommand(
    Guid Id) : IRequest<Unit>;
using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.CreateTenant;

public record CreateTenantCommand(
    Guid UserId,
    string CompanyName,
    string SubDomain,
    Guid PlanId,
    string PriceId) : ICommand<Unit>;
using MultiTenantEcommerce.Shared.Domain;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Events;

public record TenantRegisteredEvent(Guid TenantId) : DomainEvent(TenantId);
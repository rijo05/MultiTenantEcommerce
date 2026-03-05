using MultiTenantEcommerce.Shared.Domain.Events;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Events;

public record TenantRegisteredEvent(Guid TenantId) : DomainEvent(TenantId);
using MultiTenantEcommerce.Shared.Domain;

namespace MultiTenantEcommerce.Domain.Commerce.Catalog.Events;

public record ProductCreatedEvent(
    Guid TenantId, 
    Guid ProductId) : DomainEvent(TenantId);

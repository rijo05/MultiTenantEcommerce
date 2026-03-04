using MultiTenantEcommerce.Shared.Domain;

namespace MultiTenantEcommerce.Domain.Commerce.Catalog.Events;

public record ProductImageDeletedEvent(
    Guid TenantId, 
    Guid ProductId, 
    Guid ImageId, 
    string ImageKey) : DomainEvent(TenantId);

using MultiTenantEcommerce.Shared.Domain;

namespace MultiTenantEcommerce.Domain.Commerce.Inventory.Events;

public record StockLevelChangedEvent(
    Guid TenantId,
    Guid ProductId,
    int AvailableAfter,
    int MinAfter) : DomainEvent(TenantId);
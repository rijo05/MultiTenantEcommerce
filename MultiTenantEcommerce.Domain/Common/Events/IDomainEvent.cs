using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Common.Events;

public interface IDomainEvent
{
    Guid EventId { get; }
    Guid TenantId { get; }
    EventPriority EventPriority { get; }
    DateTime OccurredOn { get; }
}


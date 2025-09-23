namespace MultiTenantEcommerce.Domain.Common.Events;

public interface IDomainEvent
{
    Guid EventId { get; }
    Guid TenantId { get; }
    DateTime OccurredOn { get; }
}


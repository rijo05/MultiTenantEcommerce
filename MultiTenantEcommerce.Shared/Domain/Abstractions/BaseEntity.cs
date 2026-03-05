using MultiTenantEcommerce.Shared.Domain.Events;

namespace MultiTenantEcommerce.Shared.Domain.Abstractions;

public abstract class BaseEntity : IHasDomainEvents
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void SetCreatedAt(DateTime transactionTime)
    {
        if (CreatedAt == default)
            CreatedAt = transactionTime;
    }

    public void SetUpdatedAt(DateTime transactionTime)
    {
        UpdatedAt = transactionTime;
    }
}
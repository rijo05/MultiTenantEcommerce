using MultiTenantEcommerce.Domain.Common.Events;

namespace MultiTenantEcommerce.Domain.Common.Entities;
public abstract class BaseEntity : IHasDomainEvents
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        SetCreatedAt();
        SetUpdatedAt();
    }

    public void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    private void SetCreatedAt()
    {
        CreatedAt = DateTime.UtcNow;
    }
}

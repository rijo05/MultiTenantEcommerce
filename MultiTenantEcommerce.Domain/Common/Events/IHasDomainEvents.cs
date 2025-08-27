namespace MultiTenantEcommerce.Domain.Common.Events;

//Interface para as entities poderem guardar eventos

public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}

namespace MultiTenantEcommerce.Domain.Common;

//Interface para as entities poderem guardar eventos

public interface IHasDomainEvents
{
    List<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}

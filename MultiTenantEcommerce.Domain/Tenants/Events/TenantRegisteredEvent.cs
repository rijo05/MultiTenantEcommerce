using MultiTenantEcommerce.Domain.Common.Events;

namespace MultiTenantEcommerce.Domain.Tenants.Events;
public record TenantRegisteredEvent(
    Guid TenantId,
    string Email) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "TenantRegistered";
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

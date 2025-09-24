using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Tenants.Events;
public record TenantRegisteredEvent(
    Guid TenantId,
    string Email) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "TenantRegistered";
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.Critical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

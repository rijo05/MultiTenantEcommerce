using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Users.Events;
public record CustomerRegisteredEvent(
    Guid TenantId,
    Guid CustomerId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "CustomerRegistered";
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.Critical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

using MultiTenantEcommerce.Domain.Common.Events;

namespace MultiTenantEcommerce.Domain.Users.Events;
public record CustomerRegisteredEvent(
    Guid TenantId,
    Guid CustomerId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "CustomerRegistered";
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Users.Events;
public record EmployeeRegisteredEvent(
    Guid TenantId,
    Guid EmployeeId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "EmployeeRegistered";
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.Critical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

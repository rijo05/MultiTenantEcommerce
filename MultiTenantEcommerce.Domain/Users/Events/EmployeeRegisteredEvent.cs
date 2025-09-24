using MultiTenantEcommerce.Domain.Common.Events;

namespace MultiTenantEcommerce.Domain.Users.Events;
public record EmployeeRegisteredEvent(
    Guid TenantId,
    Guid EmployeeId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "EmployeeRegistered";
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

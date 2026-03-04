namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Events;

public record TenantMemberRegisteredEvent(
    Guid TenantId,
    Guid TenantMemberId) : IDomainEvent, IEmailEvent
{
    public string TemplateName => "TenantMemberRegistered";
    public Guid EventId { get; init; } = Guid.NewGuid();
    public EventPriority EventPriority => EventPriority.Critical;
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
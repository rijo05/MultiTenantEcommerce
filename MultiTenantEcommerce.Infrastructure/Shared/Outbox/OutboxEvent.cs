using System.Reflection;
using System.Text.Json;
using MultiTenantEcommerce.Shared.Domain.Events;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Integration.Events;

namespace MultiTenantEcommerce.Infrastructure.Shared.Outbox;

public class OutboxEvent
{
    private OutboxEvent()
    {
    }

    public OutboxEvent(IIntegrationEvent integrationEvent)
    {
        Id = integrationEvent.EventId;
        TenantId = integrationEvent.TenantId;
        OccurredOn = integrationEvent.OccurredOn;

        Type = integrationEvent.GetType().AssemblyQualifiedName!;
        Content = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType());

        Retries = 0;

        var isHighPriority = integrationEvent.GetType().IsDefined(typeof(HighPriorityAttribute), false);
        Priority = isHighPriority ? 9 : 1;
    }

    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Type { get; private set; }
    public string Content { get; private set; }

    public int Priority { get; private set; }

    public DateTime OccurredOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }
    public string? Error { get; private set; }
    public int Retries { get; private set; }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
    }

    public void SetErrors(string errors)
    {
        if (string.IsNullOrEmpty(errors))
            return;

        Error = string.IsNullOrEmpty(Error) ? errors : $"{Error}\n{errors}";
    }

    public void IncrementRetries()
    {
        Retries++;
    }
}
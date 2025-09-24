using MultiTenantEcommerce.Domain.Common.Events;
using System.Text.Json;

namespace MultiTenantEcommerce.Infrastructure.Outbox;
public class OutboxEvent
{
    public Guid Id { get; private set; }
    public string Type { get; private set; }
    public string Content { get; private set; }
    public DateTime OccurredOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }
    public string? Error { get; private set; }
    public int Retries { get; private set; }

    private OutboxEvent() { }
    public OutboxEvent(IDomainEvent domainEvent)
    {
        Id = Guid.NewGuid();
        Type = domainEvent.GetType().AssemblyQualifiedName!;
        Content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType());
        OccurredOn = DateTime.UtcNow;
        Retries = 0;
    }

    public void SetProcessedOn() => ProcessedOn = DateTime.UtcNow;

    public void SetErrors(string errors)
    {
        if (string.IsNullOrEmpty(errors))
            return;

        Error = string.IsNullOrEmpty(Error) ? errors : $"{Error}\n{errors}";
    }

    public void IncrementRetries() => Retries++;
}

public class EventWrapper
{
    public string EventType { get; set; } = null!;
    public string Data { get; set; } = null!;
}

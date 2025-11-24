using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Infrastructure.Workers;
using System.Text.Json;

namespace MultiTenantEcommerce.Infrastructure.Outbox;
public class OutboxProcessor : IPriorityProcessor
{
    private readonly IOutboxRepository _outboxRepository;
    private readonly IEventBus _eventBus;
    private const int BATCH_SIZE = 20;

    public OutboxProcessor(IOutboxRepository outboxRepository,
        IEventBus eventBus)
    {
        _outboxRepository = outboxRepository;
        _eventBus = eventBus;
    }

    public async Task ExecuteAsync(EventPriority priority)
    {
        var events = await _outboxRepository.GetUnprocessedEvents(priority, BATCH_SIZE);
        Console.WriteLine($"Foram encontrados {events.Count} eventos");

        foreach (var item in events)
        {
            try
            {
                var wrapper = new EventWrapper
                {
                    EventType = item.Type,
                    Data = item.Content
                };

                var json = JsonSerializer.Serialize(wrapper);

                await _eventBus.PublishAsync(item.RoutingKey, json);

                item.SetProcessedOn();
            }
            catch (Exception ex)
            {
                item.SetErrors(ex.Message);
                item.IncrementRetries();
            }
            finally
            {
                await _outboxRepository.UpdateAsync(item);
            }
        }

        await _outboxRepository.SaveChangesAsync();
    }
}

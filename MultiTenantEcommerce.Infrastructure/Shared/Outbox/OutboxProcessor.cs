using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiTenantEcommerce.Shared.Application.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Shared.Outbox;

public class OutboxProcessor : BackgroundService
{
    private const int BATCH_SIZE = 20;
    private readonly IServiceProvider _serviceProvider;

    public OutboxProcessor(IOutboxRepository outboxRepository,
        IEventBus eventBus,
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var delayTime = 2000;

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

                var events = await outboxRepository.GetUnprocessedEvents(BATCH_SIZE);

                if (events.Any())
                {
                    foreach (var item in events)
                    {
                        try
                        {
                            await eventBus.PublishAsync(item.Content, item.Type, item.Priority);

                            item.MarkAsProcessed();
                        }
                        catch (Exception ex)
                        {
                            item.SetErrors(ex.Message);
                            item.IncrementRetries();
                        }
                    }
                    await outboxRepository.SaveChangesAsync();

                    if (events.Count == BATCH_SIZE)
                    {
                        delayTime = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                delayTime = 5000;
            }

            if (delayTime > 0)
            {
                await Task.Delay(delayTime, stoppingToken);
            }
        }
    }
}
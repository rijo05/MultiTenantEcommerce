using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Infrastructure.Workers;
public class GenericWorker<TProcessor> : BackgroundService
    where TProcessor : class, IPriorityProcessor
{
    private readonly TimeSpan _pollingInterval;
    private readonly IServiceProvider _serviceProvider;
    private readonly EventPriority _priority;

    public GenericWorker(TimeSpan pollingInterval, IServiceProvider serviceProvider, EventPriority priority)
    {
        _pollingInterval = TimeSpan.FromDays(1000);
        _serviceProvider = serviceProvider;
        _priority = priority;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var _processor = scope.ServiceProvider.GetRequiredService<TProcessor>()!;

            await _processor.ExecuteAsync(_priority);

            await Task.Delay(_pollingInterval, stoppingToken);
        }
    }
}

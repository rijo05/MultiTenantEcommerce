using Microsoft.Extensions.DependencyInjection;
using MultiTenantEcommerce.Infrastructure.Events;
using MultiTenantEcommerce.Infrastructure.Shared.Events;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using MultiTenantEcommerce.Shared.Domain.Events;
using MultiTenantEcommerce.Shared.Integration.Events;

namespace MultiTenantEcommerce.Infrastructure.Shared.Messaging;

public class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public EventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchSync(IDomainEvent domainEvent, CancellationToken ct = default)
    {
        var handlerType = typeof(ISyncHandler<>).MakeGenericType(domainEvent.GetType());
        var handlers = _serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
            await ((dynamic)handler).HandleAsync((dynamic)domainEvent);
    }

    public async Task DispatchAsync(IIntegrationEvent integrationEvent, CancellationToken ct = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;

        var tenantContext = sp.GetRequiredService<ITenantContext>();
        tenantContext.SetTenantId(integrationEvent.TenantId);

        var handlerType = typeof(IAsyncHandler<>).MakeGenericType(integrationEvent.GetType());
        var handlers = sp.GetServices(handlerType);

        var processedRepo = sp.GetRequiredService<IProcessedEventsRepository>();
        var unitOfWork = sp.GetRequiredService<IUnitOfWork>();

        foreach (var handler in handlers)
        {
            var handlerName = handler.GetType().Name;

            if (await processedRepo.WasThisEventProcessedAlready(integrationEvent.EventId, handlerName))
                continue;

            await ((dynamic)handler).HandleAsync((dynamic)integrationEvent);

            await processedRepo.AddAsync(new ProcessedEvent(integrationEvent.EventId, handlerName));

            await unitOfWork.CommitAsync();
        }
    }
}
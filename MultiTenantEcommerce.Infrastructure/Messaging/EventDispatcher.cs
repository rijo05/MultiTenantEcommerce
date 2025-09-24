using Microsoft.Extensions.DependencyInjection;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Infrastructure.Events;

namespace MultiTenantEcommerce.Infrastructure.Messaging;
public class EventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public EventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(IDomainEvent domainEvent)
    {
        using var scope = _serviceProvider.CreateScope();
        var scopedProvider = scope.ServiceProvider;

        var handlers = scopedProvider.GetServices(typeof(IEventHandler<>)
            .MakeGenericType(domainEvent.GetType()))
            ?.ToList() ?? new List<object>();

        var processedRepo = scopedProvider.GetRequiredService<IProcessedEventsRepository>();
        var unitOfWork = scopedProvider.GetRequiredService<IUnitOfWork>();
        var tenantContext = scopedProvider.GetRequiredService<ITenantContext>();
        tenantContext.SetTenantId(domainEvent.TenantId);

        foreach (var handler in handlers)
        {
            var handlerName = handler.GetType().AssemblyQualifiedName!;

            if (!await processedRepo.WasThisEventProcessedAlready(domainEvent.EventId, handlerName))
            {
                dynamic dynamicHandler = handler;
                await dynamicHandler.HandleAsync((dynamic)domainEvent);

                await processedRepo.AddAsync(new ProcessedEvent(domainEvent.EventId, handlerName));
                await unitOfWork.CommitAsync();
            }
        }
    }
}

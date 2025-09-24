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
        Console.WriteLine("Entrei no dispatch");
        using var scope = _serviceProvider.CreateScope();
        var scopedProvider = scope.ServiceProvider;

        var handlers = scopedProvider.GetServices(typeof(IEventHandler<>)
            .MakeGenericType(domainEvent.GetType()))
            ?.ToList() ?? new List<object>();

        var _processedEventsRepository = scopedProvider.GetRequiredService<IProcessedEventsRepository>();
        var _unitOfWork = scopedProvider.GetRequiredService<IUnitOfWork>();
        var _tenantContext = scopedProvider.GetRequiredService<ITenantContext>();
        _tenantContext.SetTenantId(domainEvent.TenantId);

        foreach (var handler in handlers)
        {
            var handleMethod = handler.GetType().GetMethod("HandleAsync")!;

            var task = (Task)handleMethod.Invoke(handler, new object[] { domainEvent })!;

            await task;

            var handlerName = handler.GetType().AssemblyQualifiedName!;
            if (!await _processedEventsRepository.WasThisEventProcessedAlready(domainEvent.EventId, handlerName))
            {
                await _processedEventsRepository.AddAsync(new ProcessedEvent(domainEvent.EventId, handlerName));
                await _unitOfWork.CommitAsync();
            }
        }
    }
}

using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MultiTenantEcommerce.Shared.Domain.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MultiTenantEcommerce.Infrastructure.Shared.Messaging;

public class BackgroundConsumer : BackgroundService
{
    private const string QUEUE_NAME = "background-jobs";
    private readonly IConnection _connection;
    private readonly IServiceProvider _serviceProvider;
    private IChannel? _channel;

    public BackgroundConsumer(IServiceProvider serviceProvider, IConnection connection)
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await _connection.CreateChannelAsync();

        await _channel.BasicQosAsync(0, 10, false);

        var args = new Dictionary<string, object> { { "x-max-priority", 10 } };
        await _channel.QueueDeclareAsync(QUEUE_NAME, true, false, false, args);


        await _channel.QueueBindAsync(QUEUE_NAME, "domain-events-exchange", "#");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var eventTypeStr = ea.BasicProperties.Type;

                using var scope = _serviceProvider.CreateScope();
                var dispatcher = scope.ServiceProvider.GetRequiredService<EventDispatcher>();

                var type = Type.GetType(eventTypeStr!);
                var domainEvent = JsonSerializer.Deserialize(body, type!) as IDomainEvent;

                if (domainEvent != null) await dispatcher.DispatchAsync(domainEvent, stoppingToken);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
            }
        };

        await _channel.BasicConsumeAsync(QUEUE_NAME, false, consumer);
    }
}
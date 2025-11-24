using Microsoft.Extensions.Hosting;
using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Infrastructure.Outbox;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MultiTenantEcommerce.Infrastructure.Messaging;
public class RabbitMqConsumer : BackgroundService
{
    private readonly EventDispatcher _dispatcher;
    private readonly RabbitMqConnectionManager _connectionManager;
    private IChannel _channel = null;
    private string _queue;
    private string _binding;
    private const string EXCHANGE = "domain-events-exchange";

    public RabbitMqConsumer(EventDispatcher dispatcher,
        RabbitMqConnectionManager connectionManager,
        string queue,
        string binding)
    {
        _dispatcher = dispatcher;
        _connectionManager = connectionManager;
        _queue = queue;
        _binding = binding;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"[x] Starting consumer for queue {_queue}");

        var connection = await _connectionManager.GetConnectionAsync();
        _channel = await connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(exchange: EXCHANGE,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);


        await _channel.QueueDeclareAsync(queue: _queue,
            durable: true,
            exclusive: false,
            autoDelete: false);


        await _channel.QueueBindAsync(queue: _queue,
            exchange: EXCHANGE,
            routingKey: _binding);

        Console.WriteLine($"[✓] Queue {_queue} declared and bound with {_binding}");

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            var wrapper = JsonSerializer.Deserialize<EventWrapper>(json);
            if (wrapper == null)
                return;

            var eventType = Type.GetType(wrapper.EventType);
            if (eventType == null)
            {
                Console.WriteLine($"Tipo não encontrado: {wrapper.EventType}");
                await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                return;
            }

            var domainEvent = (IDomainEvent)JsonSerializer.Deserialize(
                wrapper.Data,
                Type.GetType(wrapper.EventType)!
            )!;

            if (domainEvent != null)
                await _dispatcher.DispatchAsync(domainEvent);

            await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
        };


        await _channel.BasicConsumeAsync(queue: _queue, autoAck: false, consumer: consumer);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null) await _channel.CloseAsync();
        await base.StopAsync(cancellationToken);
    }
}

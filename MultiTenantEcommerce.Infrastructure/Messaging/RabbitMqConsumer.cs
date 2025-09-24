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
    private const string QueueName = "domain-events-queue";
    private const string Exchange = "domain-events";

    public RabbitMqConsumer(EventDispatcher dispatcher,
        RabbitMqConnectionManager connectionManager)
    {
        _dispatcher = dispatcher;
        _connectionManager = connectionManager;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var connection = await _connectionManager.GetConnectionAsync();
        _channel = await connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(exchange: Exchange, type: ExchangeType.Fanout, durable: true, autoDelete: false);
        await _channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: false);
        await _channel.QueueBindAsync(queue: QueueName, exchange: Exchange, routingKey: "");

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            var wrapper = JsonSerializer.Deserialize<EventWrapper>(json);
            if (wrapper == null) return;

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

            Console.WriteLine("ATENCAOOOOOOOOOOO!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            // Envia para o dispatcher
            if (domainEvent != null)
                await _dispatcher.DispatchAsync(domainEvent);

            await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
            Console.WriteLine("Validei1");
        };


        await _channel.BasicConsumeAsync(queue: QueueName, autoAck: false, consumer: consumer);
        Console.WriteLine("Validei2");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null) await _channel.CloseAsync();
        await base.StopAsync(cancellationToken);
    }
}

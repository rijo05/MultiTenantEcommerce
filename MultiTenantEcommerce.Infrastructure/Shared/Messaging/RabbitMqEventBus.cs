using System.Text;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using MultiTenantEcommerce.Shared.Domain.Utilities.Constants;
using RabbitMQ.Client;

namespace MultiTenantEcommerce.Infrastructure.Shared.Messaging;

public class RabbitMqEventBus : IEventBus
{
    private const string EXCHANGE = "domain-events-exchange";
    private readonly RabbitMqConnectionManager _connectionManager;
    private IChannel? _channel;

    public RabbitMqEventBus(RabbitMqConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task EnsureConnectionAsync()
    {
        if (_channel != null && _channel.IsOpen) return;

        var connection = await _connectionManager.GetConnectionAsync();
        _channel = await connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            EXCHANGE,
            ExchangeType.Topic,
            true,
            false
        );
    }

    public async Task PublishAsync(string content, string eventType, int priority)
    {
        await EnsureConnectionAsync();

        var body = Encoding.UTF8.GetBytes(content);

        var props = new BasicProperties
        {
            Persistent = true,
            Type = eventType,
            Priority = (byte)priority,
            DeliveryMode = DeliveryModes.Persistent,
            ContentType = MimeTypes.JSON,
            MessageId = Guid.NewGuid().ToString(),
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };

        var routingKey = eventType.Split(',')[0].Split('.').Last().ToLower();

        await _channel!.BasicPublishAsync(
            EXCHANGE,
            routingKey,
            true,
            props,
            body);
    }
}
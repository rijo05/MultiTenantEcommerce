using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using RabbitMQ.Client;
using System.Text;
namespace MultiTenantEcommerce.Infrastructure.Messaging;
public class RabbitMqEventBus : IEventBus
{
    private readonly RabbitMqConnectionManager _connectionManager;
    private IConnection _connection = null;
    private IChannel _channel = null;
    private const string EXCHANGE = "domain-events-exchange";

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
            exchange: EXCHANGE,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false
        );
    }

    public async Task PublishAsync(string routingKey, string message)
    {
        if (message == null)
            throw new Exception("message cant be null");

        await EnsureConnectionAsync();

        var body = Encoding.UTF8.GetBytes(message);

        var props = new BasicProperties()
        {
            DeliveryMode = DeliveryModes.Persistent,
            ContentType = "application/json",
            MessageId = Guid.NewGuid().ToString(),
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds()),
        };


        await _channel.BasicPublishAsync(exchange: EXCHANGE, 
            routingKey: routingKey, 
            mandatory: true, 
            basicProperties: props, 
            body: body);

        return;
    }

    public async Task Dispose()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}

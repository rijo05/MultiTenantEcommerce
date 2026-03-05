using RabbitMQ.Client;

namespace MultiTenantEcommerce.Infrastructure.Shared.Messaging;

public class RabbitMqConnectionManager : IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection;

    public RabbitMqConnectionManager()
    {
        _factory = new ConnectionFactory { HostName = "localhost" };
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_connection == null || !_connection.IsOpen) _connection = await _factory.CreateConnectionAsync();
        return _connection;
    }
}
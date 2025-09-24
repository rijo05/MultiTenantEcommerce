using RabbitMQ.Client;

namespace MultiTenantEcommerce.Infrastructure.Messaging;
public class RabbitMqConnectionManager : IAsyncDisposable
{
    private IConnection _connection;
    private readonly ConnectionFactory _factory;

    public RabbitMqConnectionManager()
    {
        _factory = new ConnectionFactory { HostName = "localhost" };
    }

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            _connection = await _factory.CreateConnectionAsync();
        }
        return _connection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
    }
}

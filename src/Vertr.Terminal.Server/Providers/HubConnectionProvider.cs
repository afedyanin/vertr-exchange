using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;

namespace Vertr.Terminal.Server.Providers;

public class HubConnectionProvider : IAsyncDisposable
{
    private readonly HubConnection _connection;
    private readonly ILogger<HubConnectionProvider> _logger;

    public HubConnectionProvider(ILogger<HubConnectionProvider> logger)
    {
        _logger = logger;

        var connection = new HubConnectionBuilder()
            .WithUrl(
            url: "http://localhost:5000/exchange",
            transports: HttpTransportType.WebSockets,
            options =>
            {
                options.SkipNegotiation = true;
            })
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
            })
            .AddMessagePackProtocol()
            .Build();

        _connection = connection;

        // TODO: Add Event Handlers
    }

    public async Task<HubConnection> GetConnection()
    {
        if (_connection.State == HubConnectionState.Disconnected)
        {
            _logger.LogInformation("Starting hub connection.");
            await _connection.StartAsync();
            _logger.LogInformation("Hub connection started");
        }

        return _connection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            if (_connection.State == HubConnectionState.Connected)
            {
                await _connection.StopAsync();
            }

            await _connection.DisposeAsync();
        }
    }
}

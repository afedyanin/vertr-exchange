using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vertr.Terminal.ExchangeClient.Configuration;
using Vertr.Terminal.ExchangeClient.Extensions;

namespace Vertr.Terminal.ExchangeClient.Providers;

internal sealed class HubConnectionProvider : IHubConnectionProvider, IAsyncDisposable
{
    private readonly HubConnection _connection;
    private readonly ExchangeConfiguration _configuration;
    private readonly ILogger<HubConnectionProvider> _logger;

    public HubConnectionProvider(
        IOptions<ExchangeConfiguration> configuration,
        ILogger<HubConnectionProvider> logger)
    {
        _logger = logger;
        _configuration = configuration.Value;

        var connection = new HubConnectionBuilder()
            .WithUrl(
            url: _configuration.HubConnectionBaseUrl,
            transports: HttpTransportType.WebSockets,
            options =>
            {
                options.SkipNegotiation = true;
            })
            .ConfigureLogging(logging =>
            {
                logging.AddProvider(_logger.AsLoggerProvider());
                // logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
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

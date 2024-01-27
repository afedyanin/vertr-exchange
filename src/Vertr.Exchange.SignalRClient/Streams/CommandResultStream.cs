using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.SignalRClient.Awaiting;
using Vertr.Exchange.SignalRClient.Providers;

namespace Vertr.Exchange.SignalRClient.Streams;

internal sealed class CommandResultStream(
    IHubConnectionProvider hubConnectionProvider,
    ICommandAwaitingService commandAwaitingService,
    ILogger<CommandResultStream> logger) : BackgroundService
{
    private readonly IHubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly ILogger<CommandResultStream> _logger = logger;
    private readonly ICommandAwaitingService _commandAwaitingService = commandAwaitingService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening API Command result stream...");

        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<ApiCommandResult>("ApiCommandResults", stoppingToken);

        while (await channel.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var apiCommandResult))
            {
                _logger.LogDebug("API Commad result received: {commandResult}", apiCommandResult);
                var resp = new CommandResponse(apiCommandResult);
                _commandAwaitingService.Complete(resp);
            }
        }
    }
}

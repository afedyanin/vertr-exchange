using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Terminal.Server.Awaiting;
using Vertr.Exchange.Terminal.Server.Providers;

namespace Vertr.Exchange.Terminal.Server.BackgroundServices;

public class CommandResultService(
    HubConnectionProvider hubConnectionProvider,
    ICommandAwaitingService commandAwaitingService,
    ILogger<CommandResultService> logger) : BackgroundService
{
    private readonly HubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly ILogger<CommandResultService> _logger = logger;
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
                _logger.LogInformation($"API Commad result received. OrderId={apiCommandResult.OrderId} ResultCode={apiCommandResult.ResultCode}");
                var resp = new CommandResponse(apiCommandResult);
                _commandAwaitingService.Complete(resp);
            }
        }
    }
}

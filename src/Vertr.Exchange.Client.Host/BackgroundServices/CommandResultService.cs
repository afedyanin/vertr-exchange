using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Client.Host.Providers;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Client.Host.BackgroundServices;

public class CommandResultService(
    HubConnectionProvider hubConnectionProvider,
    ILogger<CommandResultService> logger) : BackgroundService
{
    private readonly HubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly ILogger<CommandResultService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening API Command result stream...");
        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<ApiCommandResult>("ApiCommandResults", CancellationToken.None);
        while (await channel.WaitToReadAsync() && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var apiCommandResult))
            {
                _logger.LogInformation($"API Commad result received. OrderId={apiCommandResult.OrderId} ResultCode={apiCommandResult.ResultCode}");
            }
        }
    }
}

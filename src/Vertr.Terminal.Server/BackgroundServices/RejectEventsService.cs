using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.Server.OrderManagement;
using Vertr.Terminal.Server.Providers;

namespace Vertr.Terminal.Server.BackgroundServices;

public class RejectEventsService(
    HubConnectionProvider hubConnectionProvider,
    IOrderEventHandler orderEventHandler,
    ILogger<RejectEventsService> logger) : BackgroundService
{
    private readonly HubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly IOrderEventHandler _orderEventHandler = orderEventHandler;
    private readonly ILogger<RejectEventsService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening Reject Events stream...");
        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<RejectEvent>("RejectEvents", stoppingToken);
        while (await channel.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var rejectEvent))
            {
                await _orderEventHandler.HandleRejectEvent(rejectEvent);
                _logger.LogDebug("Reject Event received: {rejectEvent}", rejectEvent);
            }
        }
    }
}

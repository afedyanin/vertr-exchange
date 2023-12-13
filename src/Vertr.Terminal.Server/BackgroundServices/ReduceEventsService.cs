using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.Server.OrderManagement;
using Vertr.Terminal.Server.Providers;

namespace Vertr.Terminal.Server.BackgroundServices;

public class ReduceEventsService(
    HubConnectionProvider hubConnectionProvider,
    IOrderEventHandler orderEventHandler,
    ILogger<ReduceEventsService> logger) : BackgroundService
{
    private readonly HubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly IOrderEventHandler _orderEventHandler = orderEventHandler;
    private readonly ILogger<ReduceEventsService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening Reduce Events stream...");
        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<ReduceEvent>("ReduceEvents", stoppingToken);
        while (await channel.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var reduceEvent))
            {
                await _orderEventHandler.HandleReduceEvent(reduceEvent);
                _logger.LogInformation("Reduce Event received: {reduceEvent}", reduceEvent);
            }
        }
    }
}

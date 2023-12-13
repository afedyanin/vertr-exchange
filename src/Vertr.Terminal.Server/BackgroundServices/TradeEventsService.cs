using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.Server.OrderManagement;
using Vertr.Terminal.Server.Providers;
using Vertr.Terminal.Server.Repositories;

namespace Vertr.Terminal.Server.BackgroundServices;

public class TradeEventsService(
    HubConnectionProvider hubConnectionProvider,
    ITradeEventsRepository tradeEventsRepository,
    IOrderEventHandler orderEventHandler,
    ILogger<TradeEventsService> logger) : BackgroundService
{
    private readonly HubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly ITradeEventsRepository _tradeEventsRepository = tradeEventsRepository;
    private readonly ILogger<TradeEventsService> _logger = logger;
    private readonly IOrderEventHandler _orderEventHandler = orderEventHandler;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening Trade Events stream...");
        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<TradeEvent>("TradeEvents", stoppingToken);
        while (await channel.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var tradeEvent))
            {
                await _tradeEventsRepository.Save(tradeEvent);
                await _orderEventHandler.HandleTradeEvent(tradeEvent);

                _logger.LogInformation("Trade Event received: {tradeEvent}", tradeEvent);
            }
        }
    }
}


using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.Server.Converters;
using Vertr.Terminal.Server.Providers;
using Vertr.Terminal.Server.Repositories;

namespace Vertr.Terminal.Server.BackgroundServices;

public class TradeEventsService(
    HubConnectionProvider hubConnectionProvider,
    ITradeEventsRepository tradeEventsRepository,
    ILogger<TradeEventsService> logger) : BackgroundService
{
    private readonly HubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly ITradeEventsRepository _tradeEventsRepository = tradeEventsRepository;
    private readonly ILogger<TradeEventsService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening Trade Events stream...");
        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<TradeEvent>("TradeEvents", stoppingToken);
        while (await channel.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var tradeEvent))
            {
                await _tradeEventsRepository.Save(tradeEvent.ToTradeItems());
                _logger.LogInformation("Trade Event received: {tradeEvent}", tradeEvent);
            }
        }
    }
}


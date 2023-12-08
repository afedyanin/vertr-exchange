using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.Server.Providers;
using Vertr.Terminal.Server.Repositories;

namespace Vertr.Terminal.Server.BackgroundServices;

public class OrderBooksService(
    HubConnectionProvider hubConnectionProvider,
    IOrderBookSnapshotsRepository orderBookRepository,
    ILogger<OrderBooksService> logger) : BackgroundService
{
    private readonly HubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly IOrderBookSnapshotsRepository _orderBookRepository = orderBookRepository;
    private readonly ILogger<OrderBooksService> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening Order Books stream...");
        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<OrderBook>("OrderBooks", stoppingToken);
        while (await channel.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var orderBook))
            {
                await _orderBookRepository.Save(orderBook);
                _logger.LogInformation("Order Book received: {orderBook}", orderBook);
            }
        }
    }
}

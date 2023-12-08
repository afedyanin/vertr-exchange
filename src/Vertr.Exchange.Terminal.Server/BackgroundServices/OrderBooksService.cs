using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Terminal.Server.Providers;
using Vertr.Exchange.Terminal.Server.Repositories;

namespace Vertr.Exchange.Terminal.Server.BackgroundServices;

public class OrderBooksService(
    HubConnectionProvider hubConnectionProvider,
    IOrderBookRepository orderBookRepository,
    ILogger<CommandResultService> logger) : BackgroundService
{
    private readonly HubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly IOrderBookRepository _orderBookRepository = orderBookRepository;
    private readonly ILogger<CommandResultService> _logger = logger;

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

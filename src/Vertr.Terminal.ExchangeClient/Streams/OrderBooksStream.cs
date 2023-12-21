using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.Application.StreamEvents;
using Vertr.Terminal.ExchangeClient.Providers;

namespace Vertr.Terminal.ExchangeClient.Streams;

internal sealed class OrderBooksStream(
    IHubConnectionProvider hubConnectionProvider,
    IMediator mediator,
    ILogger<OrderBooksStream> logger) : BackgroundService
{
    private readonly IHubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<OrderBooksStream> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening Order Book stream...");

        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<OrderBook>("OrderBooks", stoppingToken);

        while (await channel.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var orderBook))
            {
                var evt = new OrderBookRequest
                {
                    OrderBook = orderBook,
                };

                await _mediator.Send(evt);
            }
        }
    }
}

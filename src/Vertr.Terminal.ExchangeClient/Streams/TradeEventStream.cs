using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.Application.StreamEvents.Orders;
using Vertr.Terminal.ExchangeClient.Providers;

namespace Vertr.Terminal.ExchangeClient.Streams;

internal sealed class TradeEventStream(
    IHubConnectionProvider hubConnectionProvider,
    IMediator mediator,
    ILogger<TradeEventStream> logger) : BackgroundService
{
    private readonly IHubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly ILogger<TradeEventStream> _logger = logger;
    private readonly IMediator _mediator = mediator;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening Trade Events stream...");
        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<TradeEvent>("TradeEvents", stoppingToken);
        while (await channel.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var tradeEvent))
            {
                var evt = new TradeRequest
                {
                    TradeEvent = tradeEvent,
                };

                await _mediator.Send(evt);
            }
        }
    }
}


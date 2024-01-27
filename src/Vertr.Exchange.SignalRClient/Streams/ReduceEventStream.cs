using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.SignalRClient.Providers;
using Vertr.Exchange.SignalRClient.Requests;

namespace Vertr.Exchange.SignalRClient.Streams;

internal sealed class ReduceEventStream(
    IHubConnectionProvider hubConnectionProvider,
    IMediator mediator,
    ILogger<ReduceEventStream> logger) : BackgroundService
{
    private readonly IHubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly ILogger<ReduceEventStream> _logger = logger;
    private readonly IMediator _mediator = mediator;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening Reduce Events stream...");

        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<ReduceEvent>("ReduceEvents", stoppingToken);

        while (await channel.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var reduceEvent))
            {
                var evt = new HandleReduceRequest
                {
                    ReduceEvent = reduceEvent,
                };

                await _mediator.Send(evt);
            }
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Client.SignalR.Providers;
using Vertr.Exchange.Client.SignalR.Requests;

namespace Vertr.Exchange.Client.SignalR.Streams;

internal sealed class RejectEventStream(
    IHubConnectionProvider hubConnectionProvider,
    IMediator mediator,
    ILogger<RejectEventStream> logger) : BackgroundService
{
    private readonly IHubConnectionProvider _connectionProvider = hubConnectionProvider;
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<RejectEventStream> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Start listening Reject Events stream...");
        var connection = await _connectionProvider.GetConnection();
        var channel = await connection.StreamAsChannelAsync<RejectEvent>("RejectEvents", stoppingToken);
        while (await channel.WaitToReadAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            while (channel.TryRead(out var rejectEvent))
            {
                var evt = new HandleRejectRequest
                {
                    RejectEvent = rejectEvent,
                };

                await _mediator.Send(evt);
            }
        }
    }
}

using Microsoft.Extensions.Options;
using NATS.Client.Core;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Messages;
using Vertr.Exchange.Server.Configuration;
using Vertr.Exchange.Server.Extensions;

namespace Vertr.Exchange.Server.MessageHandlers;

public class NatsMessageHandler : IMessageHandler, IAsyncDisposable
{
    private readonly NatsConnection _conn;
    private readonly ILogger<NatsMessageHandler> _logger;

    public NatsMessageHandler(
        IOptions<NatsConfiguration> natsOptions,
        ILogger<NatsMessageHandler> logger)
    {
        _logger = logger;

        var serverUrl = natsOptions.Value.NatsServerUrl;
        var opts = NatsOpts.Default with { Url = serverUrl };

        _logger.LogDebug("Connecting to NATS server: {ServerUrl}", serverUrl);
        _conn = new NatsConnection(opts);
    }

    public async Task CommandResult(ApiCommandResult apiCommandResult)
    {
        _logger.LogDebug("Publish ApiCommandResult");

        await _conn.PublishAsync($"commands.{apiCommandResult.OrderId}", apiCommandResult.ToProto());
    }

    public async Task OrderBook(OrderBook orderBook)
    {
        _logger.LogDebug("Publish OrderBook");
        await _conn.PublishAsync("orderbooks", orderBook.ToProto());
    }

    public async Task ReduceEvent(ReduceEvent reduceEvent)
    {
        _logger.LogDebug("Publish ReduceEvent");
        await _conn.PublishAsync("reduces", reduceEvent.ToProto());
    }

    public async Task RejectEvent(RejectEvent rejectEvent)
    {
        _logger.LogDebug("Publish RejectEvent");
        await _conn.PublishAsync("rejects", rejectEvent.ToProto());
    }

    public async Task TradeEvent(TradeEvent tradeEvent)
    {
        _logger.LogDebug("Publish TradeEvent");
        await _conn.PublishAsync("trades", tradeEvent.ToProto());
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogDebug("Disconnecting from NATS server.");
        await _conn.DisposeAsync();
    }
}

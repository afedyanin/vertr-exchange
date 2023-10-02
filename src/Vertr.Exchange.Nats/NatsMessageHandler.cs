using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NATS.Client.Core;
using Vertr.Exchange.Messages;

namespace Vertr.Exchange.Nats;

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
        await _conn.PublishAsync($"commands.{apiCommandResult.OrderId}", apiCommandResult);
    }

    public async Task OrderBook(OrderBook orderBook)
    {
        _logger.LogDebug("Publish OrderBook");
        await _conn.PublishAsync("orderbooks", orderBook);
    }

    public async Task ReduceEvent(ReduceEvent reduceEvent)
    {
        _logger.LogDebug("Publish ReduceEvent");
        await _conn.PublishAsync("reduces", reduceEvent);
    }

    public async Task RejectEvent(RejectEvent rejectEvent)
    {
        _logger.LogDebug("Publish RejectEvent");
        await _conn.PublishAsync("rejects", rejectEvent);
    }

    public async Task TradeEvent(TradeEvent tradeEvent)
    {
        _logger.LogDebug("Publish TradeEvent");
        await _conn.PublishAsync("trades", tradeEvent);
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogDebug("Disconnecting from NATS server.");
        await _conn.DisposeAsync();
    }
}

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

    public void CommandResult(ApiCommandResult apiCommandResult)
    {
        _logger.LogDebug("Publish ApiCommandResult");
        var vTask = _conn.PublishAsync("command", apiCommandResult);
    }

    public void OrderBook(OrderBook orderBook)
    {
        _logger.LogDebug("Publish OrderBook");
        var vTask = _conn.PublishAsync("orderbook", orderBook);
    }

    public void ReduceEvent(ReduceEvent reduceEvent)
    {
        _logger.LogDebug("Publish ReduceEvent");
        var vTask = _conn.PublishAsync("reduce", reduceEvent);
    }

    public void RejectEvent(RejectEvent rejectEvent)
    {
        _logger.LogDebug("Publish RejectEvent");
        var vTask = _conn.PublishAsync("reject", rejectEvent);
    }

    public void TradeEvent(TradeEvent tradeEvent)
    {
        _logger.LogDebug("Publish TradeEvent");
        var vTask = _conn.PublishAsync("trade", tradeEvent);
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogDebug("Disconnecting from NATS server.");
        await _conn.DisposeAsync();
    }
}

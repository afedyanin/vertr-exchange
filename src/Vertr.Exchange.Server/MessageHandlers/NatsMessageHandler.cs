using System.Buffers;
using Google.Protobuf;
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
    private readonly NatsConfiguration _natsConfiguration;

    public NatsMessageHandler(
        IOptions<NatsConfiguration> natsConfigOptions,
        ILogger<NatsMessageHandler> logger)
    {
        _logger = logger;
        _natsConfiguration = natsConfigOptions.Value;

        var opts = NatsOpts.Default with
        {
            Url = _natsConfiguration.ServerUrl,
        };

        _logger.LogDebug("Connecting to NATS server: {ServerUrl}", _natsConfiguration.ServerUrl);
        _conn = new NatsConnection(opts);
    }

    public async Task CommandResult(ApiCommandResult apiCommandResult)
    {
        _logger.LogDebug("Publish ApiCommandResult");
        await SendMessage($"commands.{apiCommandResult.OrderId}", apiCommandResult.ToProto().ToByteArray());
    }

    public async Task OrderBook(OrderBook orderBook)
    {
        _logger.LogDebug("Publish OrderBook");
        await SendMessage("orderbooks", orderBook.ToProto().ToByteArray());
    }

    public async Task ReduceEvent(ReduceEvent reduceEvent)
    {
        _logger.LogDebug("Publish ReduceEvent");
        await SendMessage("reduces", reduceEvent.ToProto().ToByteArray());
    }

    public async Task RejectEvent(RejectEvent rejectEvent)
    {
        _logger.LogDebug("Publish RejectEvent");
        await SendMessage("rejects", rejectEvent.ToProto().ToByteArray());
    }

    public async Task TradeEvent(TradeEvent tradeEvent)
    {
        _logger.LogDebug("Publish TradeEvent");
        await SendMessage("trades", tradeEvent.ToProto().ToByteArray());
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogDebug("Disconnecting from NATS server.");
        await _conn.DisposeAsync();
    }

    private async Task SendMessage(string subject, byte[] data)
    {
        try
        {
            await _conn.PublishAsync(subject, new ReadOnlySequence<byte>(data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to NATS. Subject={Subject}", subject);
        }
    }
}

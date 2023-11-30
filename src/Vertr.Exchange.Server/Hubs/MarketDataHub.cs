using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using Vertr.Exchange.Common.Messages;
using Vertr.Exchange.Server.Extensions;
using Vertr.Exchange.Server.MessageHandlers;

namespace Vertr.Exchange.Server.Hubs;

public class MarketDataHub : Hub
{
    private readonly IObservableMessageHandler _messageHandler;

    private const int _maxBufferSize = 10;

    public MarketDataHub(IObservableMessageHandler messageHandler)
    {
        _messageHandler = messageHandler;
    }

    public ChannelReader<ApiCommandResult> ApiCommandResults()
        => _messageHandler.ApiCommandResultStream().AsChannelReader(_maxBufferSize);

    public ChannelReader<OrderBook> OrderBooks()
        => _messageHandler.OrderBookStream().AsChannelReader(_maxBufferSize);

    public ChannelReader<ReduceEvent> ReduceEvents()
        => _messageHandler.ReduceEventStream().AsChannelReader(_maxBufferSize);

    public ChannelReader<RejectEvent> RejectEvents()
        => _messageHandler.RejectEventStream().AsChannelReader(_maxBufferSize);

    public ChannelReader<TradeEvent> TradeEvents()
        => _messageHandler.TradeEventStream().AsChannelReader(_maxBufferSize);
}

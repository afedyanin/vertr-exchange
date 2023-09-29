using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Messages;

namespace Vertr.Exchange.Api.Factories;
internal static class MessageFactory
{
    public static ApiCommandResult CreateApiCommandResult(OrderCommand cmd, long seq)
        => new ApiCommandResult
        {
            OrderId = cmd.OrderId,
            Uid = cmd.Uid,
            ResultCode = (CommandResultCode)cmd.ResultCode,
            Seq = seq,
            Timestamp = cmd.Timestamp,
        };

    public static RejectEvent CreateRejectEvent(OrderCommand cmd, IEngineEvent evt)
        => new RejectEvent
        {
            OrderId = cmd.OrderId,
            Price = evt.Price,
            Symbol = cmd.Symbol,
            Timestamp = cmd.Timestamp,
            Uid = cmd.Uid,
            RejectedVolume = evt.Size
        };

    public static ReduceEvent CreateReduceEvent(OrderCommand cmd, IEngineEvent evt)
        => new ReduceEvent
        {
            OrderId = cmd.OrderId,
            Price = evt.Price,
            Symbol = cmd.Symbol,
            Timestamp = cmd.Timestamp,
            Uid = cmd.Uid,
            OrderCompleted = evt.ActiveOrderCompleted,
            ReducedVolume = evt.Size,
        };

    public static TradeEvent CreateTradeEvent(OrderCommand cmd, IEnumerable<IEngineEvent> trades)
        => new TradeEvent
        {
            // TakeOrderCompleted = evt.ActiveOrderCompleted,
            TakerAction = (OrderAction)cmd.Action!,
            TakerOrderId = cmd.OrderId,
            TakerUid = cmd.Uid,
            // TotalVolume = evt.Size, // TODO
            Symbol = cmd.Symbol,
            Timestamp = cmd.Timestamp,
            Trades = Array.Empty<Trade>() // TODO
        };

    public static OrderBook CreateOrderBook(OrderCommand cmd, L2MarketData marketData)
        => new OrderBook
        {
            Asks = CreateAsks(marketData),
            Bids = CreateBids(marketData),
            Timestamp = cmd.Timestamp,
            Symbol = cmd.Symbol,
        };

    private static IEnumerable<OrderBookRecord> CreateAsks(L2MarketData mdata)
    {
        // TODO
        return Array.Empty<OrderBookRecord>();
    }

    private static IEnumerable<OrderBookRecord> CreateBids(L2MarketData mdata)
    {
        // TODO
        return Array.Empty<OrderBookRecord>();
    }
}

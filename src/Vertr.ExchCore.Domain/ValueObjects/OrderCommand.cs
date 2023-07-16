using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.ValueObjects;

public record class OrderCommand : IOrder
{
    public long Uid { get; set; }

    public long OrderId { get; set; }

    public long Timestamp { get; set; }

    public OrderCommandType CommandType { get; set; }

    public OrderType OrderType { get; set; }

    public OrderAction Action { get; set; }

    public CommandResultCode ResultCode { get; set; }   

    public int Symbol { get; set; }

    public long Price { get; set; }

    public long Size { get; set; }

    public long ReserveBidPrice { get; set; }

    public MatcherTradeEvent? MatcherEvent { get; set; }

    public L2MarketData? L2MarketData { get; set; }

    public long Filled => 0;
}

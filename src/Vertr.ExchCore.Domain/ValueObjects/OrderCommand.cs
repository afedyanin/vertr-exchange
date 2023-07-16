using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.ValueObjects;

public record class OrderCommand
{
    public long Uid { get; set; }

    public long OrderId { get; set; }

    public long Timestamp { get; set; }

    public OrderCommandType CommandType { get; set; }

    public OrderType OrderType { get; set; }

    public OrderAction OrderAction { get; set; }

    public CommandResultCode ResultCode { get; set; }   

    public int Symbol { get; set; }

    public long Price { get; set; }

    public long Size { get; set; }

    public MatcherTradeEvent? MatcherEvent { get; set; }

    public L2MarketData? L2MarketData { get; set; }
}

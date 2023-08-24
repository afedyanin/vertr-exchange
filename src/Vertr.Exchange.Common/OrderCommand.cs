using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common;

public class OrderCommand
{
    public OrderCommandType Command { get; set; }

    public CommandResultCode ResultCode { get; set; }

    public OrderType OrderType { get; set; }

    public OrderAction? Action { get; set; }

    public L2MarketData? MarketData { get; set; }

    public IMatcherTradeEvent? MatcherEvent { get; set; }

    public long OrderId { get; set; }

    public long Uid { get; set; }

    public long Size { get; set; }

    public decimal Price { get; set; }

    public DateTime Timestamp { get; set; }

    public long Filled { get; set; }

    public int Symbol { get; set; }

    public byte[] BinaryData { get; set; } = Array.Empty<byte>();

    public BinaryDataType BinaryCommandType { get; set; }
}

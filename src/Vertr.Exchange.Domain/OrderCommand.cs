using Vertr.Exchange.Domain.Enums;

namespace Vertr.Exchange.Domain;
public class OrderCommand
{
    public OrderCommandType Command { get; set; }

    public CommandResultCode ResultCode { get; set; }

    public OrderType OrderType { get; set; }

    public OrderAction Action { get; set; }

    public L2MarketData? MarketData { get; set; }

    public MatcherTradeEvent? MatcherEvent { get; set; }

    public long OrderId { get; set; }

    public long Uid { get; set; }

    public long Size { get; set; }

    public long Price { get; set; }

    public long Timestamp { get; set; }
}

using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Contracts;
public record TradeEvent
{
    public int Symbol { get; set; }

    public long TotalVolume { get; set; }

    public long TakerOrderId { get; set; }

    public long TakerUid { get; set; }

    public OrderAction TakerAction { get; set; }

    public bool TakeOrderCompleted { get; set; }

    public DateTime Timestamp { get; set; }

    public Trade[] Trades { get; set; } = [];

    public long Seq { get; set; }
}

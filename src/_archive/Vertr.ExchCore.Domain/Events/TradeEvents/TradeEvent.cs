using Vertr.ExchCore.Domain.Enums;
using Vertr.ExchCore.Domain.ValueObjects;

namespace Vertr.ExchCore.Domain.Events.TradeEvents;

public record class TradeEvent : TradeEventBase
{
    public int Symbol { get; set; }

    public long TotalVolume { get; set; }

    public long TakerOrderId { get; set; }

    public long TakerUid { get; set; }

    public OrderAction TakerAction { get; set; }

    public bool TakeOrderCompleted { get; set; }

    public long Timestamp { get; set; }

    public IList<Trade>? Trades { get; set; }

}

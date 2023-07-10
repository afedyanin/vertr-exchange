namespace Vertr.ExchCore.Domain.Events.TradeEvents;

public record class RejectEvent
{
    public int Symbol { get; set; }

    public long RejectedVolume { get; set; }

    public long Price { get; set; }

    public long OrderId { get; set; }

    public long Uuid { get; set; }

    public long Timestamp { get; set; }
}

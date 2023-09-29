namespace Vertr.Exchange.Messages;
public record class TradeEvent
{
    public int Symbol { get; init; }
    public long TotalVolume { get; init; }
    public long TakerOrderId { get; init; }
    public long TakerUid { get; init; }
    public OrderAction TakerAction { get; init; }
    public bool TakeOrderCompleted { get; init; }
    public DateTime Timestamp { get; init; }
    public IList<Trade> Trades { get; init; } = new List<Trade>();
}

using Vertr.Exchange.Contracts.Enums;

namespace Vertr.Terminal.ApiClient.Contracts;

public record TradeItem
{
    public long Seq { get; set; }

    public int Symbol { get; set; }

    public DateTime Timestamp { get; set; }

    public OrderAction TakerAction { get; set; }

    public long TakerOrderId { get; set; }

    public long MakerOrderId { get; set; }

    public long TakerUid { get; set; }

    public long MakerUid { get; set; }

    public bool TakeOrderCompleted { get; set; }

    public bool MakerOrderCompleted { get; set; }

    public decimal Price { get; set; }

    public long Volume { get; set; }

    public long TotalVolume { get; set; }
}

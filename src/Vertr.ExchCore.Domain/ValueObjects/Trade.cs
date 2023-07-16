namespace Vertr.ExchCore.Domain.ValueObjects;

public record class Trade
{
    public long MakerOrderId { get; set; }

    public long MakerUid { get; set; }

    public bool MakerOrderCompleted { get; set; }

    public long Price { get; set; }

    public long Volume { get; set; }
}

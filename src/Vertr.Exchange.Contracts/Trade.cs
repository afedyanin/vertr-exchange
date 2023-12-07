namespace Vertr.Exchange.Contracts;

public record Trade
{
    public long MakerOrderId { get; set; }

    public long MakerUid { get; set; }

    public bool MakerOrderCompleted { get; set; }

    public decimal Price { get; set; }

    public long Volume { get; set; }
}

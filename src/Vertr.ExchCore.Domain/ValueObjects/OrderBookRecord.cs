namespace Vertr.ExchCore.Domain.ValueObjects;

public record class OrderBookRecord
{
    public long Price { get; set; }

    public long Volume { get; set; }

    public long Orders { get; set; }
}

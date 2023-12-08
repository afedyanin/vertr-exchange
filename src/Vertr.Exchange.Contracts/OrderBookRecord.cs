namespace Vertr.Exchange.Contracts;

public record OrderBookRecord
{
    public decimal Price { get; set; }

    public long Volume { get; set; }

    public long Orders { get; set; }
}

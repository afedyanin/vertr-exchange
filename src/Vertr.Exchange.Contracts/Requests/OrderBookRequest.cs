namespace Vertr.Exchange.Contracts.Requests;
public record OrderBookRequest
{
    public int Symbol { get; set; }

    public int Size { get; set; }
}

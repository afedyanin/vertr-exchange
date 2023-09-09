namespace Vertr.Exchange.Api.Commands;
public record class OrderBookRequest : ApiCommand
{
    public int Symbol { get; set; }

    public int Size { get; set; }
}

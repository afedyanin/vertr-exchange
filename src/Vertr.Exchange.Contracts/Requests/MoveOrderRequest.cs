namespace Vertr.Exchange.Contracts.Requests;

public record MoveOrderRequest
{
    public long OrderId { get; set; }

    public long UserId { get; set; }

    public int Symbol { get; set; }

    public decimal NewPrice { get; set; }
}

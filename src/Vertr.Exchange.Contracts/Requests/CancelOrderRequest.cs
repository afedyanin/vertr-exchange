namespace Vertr.Exchange.Contracts.Requests;

public record CancelOrderRequest
{
    public long OrderId { get; set; }

    public long UserId { get; set; }

    public int Symbol { get; set; }
}

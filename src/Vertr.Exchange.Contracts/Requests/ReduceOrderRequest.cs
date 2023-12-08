namespace Vertr.Exchange.Contracts.Requests;

public record ReduceOrderRequest
{
    public long OrderId { get; set; }

    public long UserId { get; set; }

    public int Symbol { get; set; }

    public long ReduceSize { get; set; }
}

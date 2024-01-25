namespace Vertr.Exchange.Domain.Common.Messages;

public record class Trade
{
    public long MakerOrderId { get; init; }
    public long MakerUid { get; init; }
    public bool MakerOrderCompleted { get; init; }
    public decimal Price { get; init; }
    public long Volume { get; init; }
}

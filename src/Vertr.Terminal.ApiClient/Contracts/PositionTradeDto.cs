using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.ApiClient.Contracts;
public record class PositionTradeDto
{
    public long Seq { get; set; }

    public DateTime Timestamp { get; set; }

    public long Uid { get; set; }

    public int Symbol { get; set; }

    public long OrderId { get; set; }

    public PositionDirection Direction { get; set; }

    public decimal Price { get; set; }

    public long Volume { get; set; }
}

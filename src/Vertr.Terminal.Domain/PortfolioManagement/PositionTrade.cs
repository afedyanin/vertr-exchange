using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.Domain.PortfolioManagement;
public record class PositionTrade
{
    public long Seq { get; init; }

    public DateTime Timestamp { get; init; }

    public long Uid { get; init; }

    public int Symbol { get; init; }

    public long OrderId { get; init; }

    public PositionDirection Direction { get; init; }

    public decimal Price { get; init; }

    public long Volume { get; init; }
}

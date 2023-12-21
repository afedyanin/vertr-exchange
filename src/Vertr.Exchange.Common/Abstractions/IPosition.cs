using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Common.Abstractions;

public interface IPosition
{
    long Uid { get; }

    int Symbol { get; }

    PositionDirection Direction { get; }

    // Size
    decimal OpenVolume { get; }

    // Realized PnL
    decimal RealizedPnL { get; }
}

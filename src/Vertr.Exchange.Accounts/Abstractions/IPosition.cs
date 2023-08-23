using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Accounts.Abstractions;

public interface IPosition
{
    long Uid { get; }

    int Symbol { get; }

    int Currency { get; }

    PositionDirection Direction { get; }

    // Size
    decimal OpenVolume { get; }

    // Realized PnL
    decimal RealizedPnL { get; }
}

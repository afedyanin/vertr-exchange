using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Domain.Common.Abstractions;

public interface IPosition
{
    long Uid { get; }

    int Symbol { get; }

    PositionDirection Direction { get; }

    // Size
    decimal OpenVolume { get; }

    decimal PnL { get; }

    decimal FixedPnL { get; }

    decimal OpenPriceSum { get; }
}

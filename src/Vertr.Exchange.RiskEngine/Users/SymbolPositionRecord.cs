using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.RiskEngine.Users;

public class SymbolPositionRecord
{
    public long Uid { get; }

    public int Symbol { get; }

    public int Currency { get; }

    public PositionDirection Direction { get; } = PositionDirection.EMPTY;

    public long OpenVolume { get; }

    public long OpenPriceSum { get; }

    public long Profit { get; }

    public long PendingSellSize { get; }

    public long PendingBuySize { get; }

    public bool IsEmpty()
        => Direction == PositionDirection.EMPTY
            && PendingSellSize == 0L
            && PendingBuySize == 0L;

}

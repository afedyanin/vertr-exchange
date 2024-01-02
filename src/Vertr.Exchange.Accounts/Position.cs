using System.Runtime.CompilerServices;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Shared.Enums;

[assembly: InternalsVisibleTo("Vertr.Exchange.Accounts.Tests")]

namespace Vertr.Exchange.Accounts;

internal class Position(long uid, int symbol) : IPosition
{
    private decimal _openPriceSum;
    private decimal _fixedPnl;

    public long Uid { get; } = uid;

    public int Symbol { get; } = symbol;

    public PositionDirection Direction { get; private set; } = PositionDirection.EMPTY;

    // Size
    public decimal OpenVolume { get; private set; }

    // Realized PnL
    public decimal RealizedPnL => _fixedPnl + (_openPriceSum * (-1));

    public bool IsEmpty => Direction == PositionDirection.EMPTY;

    public decimal GetUnrealizedPnL(decimal price)
        => ((OpenVolume * price) - _openPriceSum) * GetMultiplier(Direction);


    public void Update(OrderAction action, long size, decimal price)
    {
        // 1. Reduce opposite position accordingly (if exists)
        var sizeToOpen = TryToCloseCurrentPosition(action, size, price);

        // 2. Increase forward position accordingly (if size left in the trading event)
        if (sizeToOpen > 0)
        {
            OpenPosition(action, sizeToOpen, price);
        }
    }

    public override string ToString()
    {
#if DEBUG
        return $"Position Uid={Uid} Symbol={Symbol} Direction={Direction} OpenVol={OpenVolume} OpenPriceSum={_openPriceSum} FixedPnl={_fixedPnl} PnL={RealizedPnL}";
#else
        return $"Position Uid={Uid} Symbol={Symbol} Direction={Direction} OpenVol={OpenVolume} PnL={RealizedPnL}";
#endif
    }

    private decimal TryToCloseCurrentPosition(OrderAction action, long tradeSize, decimal tradePrice)
    {
        if (Direction == PositionDirection.EMPTY || Direction == GetByAction(action))
        {
            // the same direction - return full size
            return tradeSize;
        }

        // current position is bigger than trade size - just reduce position accordingly, don't fix profit
        if (OpenVolume > tradeSize)
        {
            OpenVolume -= tradeSize;
            _openPriceSum -= tradeSize * tradePrice;
            return decimal.Zero;
        }

        // current position smaller than trade size, can close completely and calculate profit
        _fixedPnl += ((OpenVolume * tradePrice) - _openPriceSum) * GetMultiplier(Direction);

        var sizeToOpen = tradeSize - OpenVolume;
        OpenVolume = decimal.Zero;
        _openPriceSum = decimal.Zero;
        Direction = PositionDirection.EMPTY;

        return sizeToOpen;
    }

    private void OpenPosition(OrderAction action, decimal sizeToOpen, decimal tradePrice)
    {
        if (sizeToOpen == decimal.Zero || tradePrice == decimal.Zero)
        {
            return;
        }

        Direction = GetByAction(action);
        OpenVolume += sizeToOpen;
        _openPriceSum += tradePrice * sizeToOpen;
    }

    private static PositionDirection GetByAction(OrderAction action)
    {
        return action switch
        {
            OrderAction.ASK => PositionDirection.DIR_SHORT,
            OrderAction.BID => PositionDirection.DIR_LONG,
            _ => PositionDirection.EMPTY,
        };
    }

    private static int GetMultiplier(PositionDirection direction)
    {
        return direction switch
        {
            PositionDirection.DIR_SHORT => -1,
            PositionDirection.DIR_LONG => 1,
            PositionDirection.EMPTY => 0,
            _ => 0,
        };
    }
}

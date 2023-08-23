using System.Runtime.CompilerServices;
using Vertr.Exchange.Accounts.Abstractions;
using Vertr.Exchange.Common.Enums;

[assembly: InternalsVisibleTo("Vertr.Exchange.Accounts.Tests")]

namespace Vertr.Exchange.Accounts;

internal class Position : IPosition
{
    // Buy/Sell Amount
    private decimal _openPriceSum;

    public long Uid { get; }

    public int Symbol { get; }

    public int Currency { get; }

    public PositionDirection Direction { get; private set; }

    // Size
    public decimal OpenVolume { get; private set; }

    // Realized PnL
    public decimal RealizedPnL { get; private set; }

    public bool IsEmpty
        => Direction == PositionDirection.EMPTY;

    public Position(long uid, int symbol, int currency)
    {
        Uid = uid;
        Symbol = symbol;
        Direction = PositionDirection.EMPTY;
        Currency = currency;
    }

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
        return $"Position Uid={Uid} Symbol={Symbol} Direction={Direction} OpenVol={OpenVolume} OpenPriceSum={_openPriceSum} PnL={RealizedPnL}";
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
        var sizeToOpen = tradeSize - OpenVolume;
        RealizedPnL += ((OpenVolume * tradePrice) - _openPriceSum) * GetMultiplier(Direction);

        Direction = PositionDirection.EMPTY;
        OpenVolume = decimal.Zero;
        _openPriceSum = decimal.Zero;

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

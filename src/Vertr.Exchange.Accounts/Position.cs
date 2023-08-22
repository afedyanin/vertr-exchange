using System.Runtime.CompilerServices;
using Vertr.Exchange.Common.Enums;

[assembly: InternalsVisibleTo("Vertr.Exchange.Accounts.Tests")]

namespace Vertr.Exchange.Accounts;

internal class Position
{
    public long Uid { get; }

    public int Symbol { get; }

    public PositionDirection Direction { get; private set; }

    // Current Size
    public decimal OpenVolume { get; private set; }

    // Buy/Sell Amount
    public decimal OpenPriceSum { get; private set; }

    // Realized PnL
    public decimal RealizedPnL { get; private set; }

    public bool IsEmpty()
        => Direction == PositionDirection.EMPTY;

    // Конструктор работает по спеке к символу
    public Position(long uid, int symbol)
    {
        Uid = uid;
        Symbol = symbol;
        Direction = PositionDirection.EMPTY;
    }

    public decimal GetUnrealizedPnL(decimal price)
        => ((OpenVolume * price) - OpenPriceSum) * GetMultiplier(Direction);

    public decimal Update(OrderAction action, long size, decimal price)
    {
        // 1. Reduce opposite position accordingly (if exists)
        var sizeToOpen = TryToCloseCurrentPosition(action, size, price);

        // 2. Increase forward position accordingly (if size left in the trading event)
        if (sizeToOpen > 0)
        {
            OpenPosition(action, sizeToOpen, price);
        }

        // new OpenVolume
        return sizeToOpen;
    }

    private decimal TryToCloseCurrentPosition(OrderAction action, long tradeSize, decimal tradePrice)
    {
        if (Direction == PositionDirection.EMPTY || Direction == GetByAction(action))
        {
            // The same direction - return full size
            return tradeSize;
        }

        // current position is bigger than trade size - just reduce position accordingly, don't fix profit
        if (OpenVolume > tradeSize)
        {
            OpenVolume -= tradeSize;
            OpenPriceSum -= tradeSize * tradePrice;
            // Все кол-во закрыли текущей позицией
            return decimal.Zero;
        }

        // Разворот. Закрываем полностью текущую позицию.
        // current position smaller than trade size, can close completely and calculate profit
        var sizeToOpen = tradeSize - OpenVolume;
        RealizedPnL += ((OpenVolume * tradePrice) - OpenPriceSum) * GetMultiplier(Direction);
        Direction = PositionDirection.EMPTY;
        OpenPriceSum = decimal.Zero;
        OpenVolume = decimal.Zero;

        // validateInternalState();

        // Незакрытый остаток для разворота
        return sizeToOpen;
    }

    private void OpenPosition(OrderAction action, decimal sizeToOpen, decimal tradePrice)
    {
        OpenVolume += sizeToOpen;
        OpenPriceSum += tradePrice * sizeToOpen;
        Direction = GetByAction(action);

        // validateInternalState();
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

using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.Domain.PortfolioManagement;
public class PositionPnlRecord
{
    public DateTime Timestamp { get; private set; }
    public PositionDirection Direction { get; private set; } = PositionDirection.EMPTY;
    public decimal PnL => FixedPnL + (OpenPriceSum * (-1));
    public decimal FixedPnL { get; private set; }
    public decimal OpenVolume { get; private set; }
    public decimal OpenPriceSum { get; private set; }
    public bool IsEmpty => Direction == PositionDirection.EMPTY;

    public PositionPnlRecord ApplyTrade(PositionTrade trade)
    {
        var pos = new PositionPnlRecord
        {
            Direction = Direction,
            OpenPriceSum = OpenPriceSum,
            OpenVolume = OpenVolume,
            FixedPnL = FixedPnL,
            Timestamp = trade.Timestamp,
        };

        pos.Update(trade.Direction, trade.Volume, trade.Price);

        return pos;
    }

    private void Update(PositionDirection tradeDirection, long size, decimal price)
    {
        // 1. Reduce opposite position accordingly (if exists)
        var sizeToOpen = TryToCloseCurrentPosition(tradeDirection, size, price);

        // 2. Increase forward position accordingly (if size left in the trading event)
        if (sizeToOpen > 0)
        {
            OpenPosition(tradeDirection, sizeToOpen, price);
        }
    }

    private decimal TryToCloseCurrentPosition(PositionDirection tradeDirection, long tradeSize, decimal tradePrice)
    {
        if (Direction == PositionDirection.EMPTY || Direction == tradeDirection)
        {
            // the same direction - return full size
            return tradeSize;
        }

        // current position is bigger than trade size - just reduce position accordingly, don't fix profit
        if (OpenVolume > tradeSize)
        {
            OpenVolume -= tradeSize;
            OpenPriceSum -= tradeSize * tradePrice;
            return decimal.Zero;
        }

        // current position smaller than trade size, can close completely and calculate profit
        FixedPnL += ((OpenVolume * tradePrice) - OpenPriceSum) * GetMultiplier(Direction);

        var sizeToOpen = tradeSize - OpenVolume;
        OpenVolume = decimal.Zero;
        OpenPriceSum = decimal.Zero;
        Direction = PositionDirection.EMPTY;

        return sizeToOpen;
    }

    private void OpenPosition(PositionDirection tradeDirection, decimal sizeToOpen, decimal tradePrice)
    {
        if (sizeToOpen == decimal.Zero || tradePrice == decimal.Zero)
        {
            return;
        }

        Direction = tradeDirection;
        OpenVolume += sizeToOpen;
        OpenPriceSum += tradePrice * sizeToOpen;
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

using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common.Symbols;
using Vertr.Exchange.RiskEngine.LastPriceCache;

namespace Vertr.Exchange.RiskEngine.Users;

internal class SymbolPositionRecord
{
    public long Uid { get; }

    public int Symbol { get; }

    public int Currency { get; }

    public PositionDirection Direction { get; private set; }

    public decimal OpenVolume { get; private set; }

    public decimal OpenPriceSum { get; private set; }

    public decimal Profit { get; private set; }

    public decimal PendingSellSize { get; private set; }

    public decimal PendingBuySize { get; private set; }

    public bool IsEmpty()
        => Direction == PositionDirection.EMPTY
            && PendingSellSize == 0L
            && PendingBuySize == 0L;

    public SymbolPositionRecord(long uid, int symbol, int currency)
    {
        Uid = uid;
        Symbol = symbol;
        Currency = currency;
        Direction = PositionDirection.EMPTY;
    }

    public void PendingHold(OrderAction orderAction, long size)
    {
        if (orderAction == OrderAction.ASK)
        {
            PendingSellSize += size;
        }
        else
        {
            PendingBuySize += size;
        }
    }

    public void PendingRelease(OrderAction orderAction, long size)
    {
        if (orderAction == OrderAction.ASK)
        {
            PendingSellSize -= size;
        }
        else
        {
            PendingBuySize -= size;
        }

        //        if (pendingSellSize < 0 || pendingBuySize < 0) {
        //            log.error("uid {} : pendingSellSize:{} pendingBuySize:{}", uid, pendingSellSize, pendingBuySize);
        //        }
    }

    public decimal EstimateProfit(CoreSymbolSpecification spec, LastPriceCacheRecord? lastPriceCacheRecord)
    {
        return Direction switch
        {
            PositionDirection.EMPTY => Profit,
            PositionDirection.DIR_LONG => Profit + ((lastPriceCacheRecord.HasValue && lastPriceCacheRecord.Value.BidPrice != 0)
                                    ? ((OpenVolume * lastPriceCacheRecord.Value.BidPrice) - OpenPriceSum)
                                    : spec.MarginBuy * OpenVolume),// unknown price - no liquidity - require extra margin
            PositionDirection.DIR_SHORT => Profit + ((lastPriceCacheRecord.HasValue && lastPriceCacheRecord.Value.AskPrice != long.MaxValue)
                                    ? (OpenPriceSum - (OpenVolume * lastPriceCacheRecord.Value.AskPrice))
                                    : spec.MarginSell * OpenVolume),// unknown price - no liquidity - require extra margin
            _ => throw new InvalidOperationException("Unknown direction."),
        };
    }

    public decimal CalculateRequiredMarginForFutures(CoreSymbolSpecification spec)
    {
        var specMarginBuy = spec.MarginBuy;
        var specMarginSell = spec.MarginSell;

        var signedPosition = OpenVolume * GetMultiplier(Direction);
        var currentRiskBuySize = PendingBuySize + signedPosition;
        var currentRiskSellSize = PendingSellSize - signedPosition;

        var marginBuy = specMarginBuy * currentRiskBuySize;
        var marginSell = specMarginSell * currentRiskSellSize;

        // marginBuy or marginSell can be negative, but not both of them
        return Math.Max(marginBuy, marginSell);
    }

    public decimal CalculateRequiredMarginForOrder(CoreSymbolSpecification spec, OrderAction action, long size)
    {
        var specMarginBuy = spec.MarginBuy;
        var specMarginSell = spec.MarginSell;

        var signedPosition = OpenVolume * GetMultiplier(Direction);
        var currentRiskBuySize = PendingBuySize + signedPosition;
        var currentRiskSellSize = PendingSellSize - signedPosition;

        var marginBuy = specMarginBuy * currentRiskBuySize;
        var marginSell = specMarginSell * currentRiskSellSize;

        // either marginBuy or marginSell can be negative (because of signedPosition), but not both of them
        var currentMargin = Math.Max(marginBuy, marginSell);

        if (action == OrderAction.BID)
        {
            marginBuy += spec.MarginBuy * size;
        }
        else
        {
            marginSell += spec.MarginSell * size;
        }

        // marginBuy or marginSell can be negative, but not both of them
        var newMargin = Math.Max(marginBuy, marginSell);

        return (newMargin <= currentMargin) ? -1 : newMargin;
    }

    public decimal UpdatePositionForMarginTrade(OrderAction action, long size, decimal price)
    {

        // 1. Un-hold pending size
        PendingRelease(action, size);

        // 2. Reduce opposite position accordingly (if exists)
        var sizeToOpen = CloseCurrentPositionFutures(action, size, price);

        // 3. Increase forward position accordingly (if size left in the trading event)
        if (sizeToOpen > 0)
        {
            OpenPositionMargin(action, sizeToOpen, price);
        }

        return sizeToOpen;
    }

    public override string? ToString()
    {
        return $"SPR u={Uid} sym={Symbol} cur={Currency} pos={Direction} vol={OpenVolume} price={OpenPriceSum} pnl={Profit} pendingS={PendingSellSize} pendingB={PendingBuySize}";
    }

    public void Reset()
    {
        // log.debug("records: {}, Pending B{} S{} total size: {}", records.size(), pendingBuySize, pendingSellSize, totalSize);
        PendingBuySize = 0;
        PendingSellSize = 0;

        OpenVolume = 0;
        OpenPriceSum = 0;
        Direction = PositionDirection.EMPTY;
    }

    private decimal CloseCurrentPositionFutures(OrderAction action, long tradeSize, decimal tradePrice)
    {

        // log.debug("{} {} {} {} cur:{}-{} profit={}", uid, action, tradeSize, tradePrice, position, totalSize, profit);

        if (Direction == PositionDirection.EMPTY || Direction == GetByAction(action))
        {
            // nothing to close
            return tradeSize;
        }

        if (OpenVolume > tradeSize)
        {
            // current position is bigger than trade size - just reduce position accordingly, don't fix profit
            OpenVolume -= tradeSize;
            OpenPriceSum -= tradeSize * tradePrice;
            return 0;
        }

        // current position smaller than trade size, can close completely and calculate profit
        Profit += ((OpenVolume * tradePrice) - OpenPriceSum) * GetMultiplier(Direction);
        OpenPriceSum = 0;
        Direction = PositionDirection.EMPTY;
        var sizeToOpen = tradeSize - OpenVolume;
        OpenVolume = 0;

        // validateInternalState();

        return sizeToOpen;
    }

    private void OpenPositionMargin(OrderAction action, decimal sizeToOpen, decimal tradePrice)
    {
        OpenVolume += sizeToOpen;
        OpenPriceSum += tradePrice * sizeToOpen;
        Direction = GetByAction(action);

        // validateInternalState();
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

    private static PositionDirection GetByAction(OrderAction action)
    {
        return action switch
        {
            OrderAction.ASK => PositionDirection.DIR_SHORT,
            OrderAction.BID => PositionDirection.DIR_LONG,
            _ => PositionDirection.EMPTY,
        };
    }
}

using Vertr.Exchange.Common.Symbols;

namespace Vertr.Exchange.RiskEngine.Orders;

internal static class CoreArithmeticUtils
{
    public static decimal CalculateAmountAsk(long size, CoreSymbolSpecification spec)
    {
        return size * spec.BaseScaleK;
    }

    public static decimal CalculateAmountBid(long size, decimal price, CoreSymbolSpecification spec)
    {
        return size * price * spec.QuoteScaleK;
    }

    public static decimal CalculateAmountBidTakerFee(long size, decimal price, CoreSymbolSpecification spec)
    {
        return size * (price * spec.QuoteScaleK + spec.TakerFee);
    }

    public static decimal CalculateAmountBidReleaseCorrMaker(long size, decimal priceDiff, CoreSymbolSpecification spec)
    {
        return size * (priceDiff * spec.QuoteScaleK + (spec.TakerFee - spec.MakerFee));
    }

    public static decimal CalculateAmountBidTakerFeeForBudget(long size, decimal budgetInSteps, CoreSymbolSpecification spec)
    {
        return budgetInSteps * spec.QuoteScaleK + size * spec.TakerFee;
    }
}

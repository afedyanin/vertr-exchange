using Vertr.Exchange.Common.Symbols;

namespace Vertr.Exchange.RiskEngine;

internal static class CoreArithmeticUtils
{
    public static long CalculateAmountAsk(long size, CoreSymbolSpecification spec)
    {
        return size * spec.BaseScaleK;
    }

    public static long CalculateAmountBid(long size, long price, CoreSymbolSpecification spec)
    {
        return size * price * spec.QuoteScaleK;
    }

    public static decimal CalculateAmountBidTakerFee(long size, decimal price, CoreSymbolSpecification spec)
    {
        return size * ((price * spec.QuoteScaleK) + spec.TakerFee);
    }

    public static long CalculateAmountBidReleaseCorrMaker(long size, long priceDiff, CoreSymbolSpecification spec)
    {
        return size * ((priceDiff * spec.QuoteScaleK) + (spec.TakerFee - spec.MakerFee));
    }

    public static decimal CalculateAmountBidTakerFeeForBudget(long size, decimal budgetInSteps, CoreSymbolSpecification spec)
    {

        return (budgetInSteps * spec.QuoteScaleK) + (size * spec.TakerFee);
    }
}

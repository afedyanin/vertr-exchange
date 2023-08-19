using Vertr.Exchange.Common.Symbols;

namespace Vertr.Exchange.RiskEngine.Extensions;
internal static class CoreSymbolSpecificationExtensions
{
    public static decimal CalculateAmountAsk(this CoreSymbolSpecification spec, long size)
        => size * spec.BaseScaleK;

    public static decimal CalculateAmountBid(this CoreSymbolSpecification spec, long size, decimal price)
        => size * price * spec.QuoteScaleK;

    public static decimal CalculateAmountBidTakerFee(this CoreSymbolSpecification spec, long size, decimal price)
        => size * (price * spec.QuoteScaleK + spec.TakerFee);

    public static decimal CalculateAmountBidReleaseCorrMaker(this CoreSymbolSpecification spec, long size, decimal priceDiff)
        => size * (priceDiff * spec.QuoteScaleK + (spec.TakerFee - spec.MakerFee));

    public static decimal CalculateAmountBidTakerFeeForBudget(this CoreSymbolSpecification spec, long size, decimal budgetInSteps)
        => budgetInSteps * spec.QuoteScaleK + size * spec.TakerFee;
}


namespace Vertr.Exchange.RiskEngine.Abstractions;

internal interface IOrderRiskEngineInternal
{
    bool IsMarginTradingEnabled { get; }

    bool IgnoreRiskProcessing { get; }

    IUserProfileService UserProfileService { get; }

    ISymbolSpecificationProvider SymbolSpecificationProvider { get; }

    IFeeCalculationService FeeCalculationService { get; }

    ILastPriceCacheProvider LastPriceCacheProvider { get; }

    IAdjustmentsService AdjustmentsService { get; }
}

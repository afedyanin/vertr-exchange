
using Vertr.Exchange.Accounts.Abstractions;

namespace Vertr.Exchange.RiskEngine.Abstractions;

internal interface IOrderRiskEngineInternal
{
    bool IsMarginTradingEnabled { get; }

    bool IgnoreRiskProcessing { get; }

    IUserProfilesRepository UserProfiles { get; }

    ISymbolSpecificationProvider SymbolSpecificationProvider { get; }

    IFeeCalculationService FeeCalculationService { get; }

    ILastPriceCacheProvider LastPriceCacheProvider { get; }
}

using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common;
using Vertr.Exchange.RiskEngine.Abstractions;
using Vertr.Exchange.Accounts.Abstractions;

namespace Vertr.Exchange.RiskEngine.Orders;

internal abstract class RiskEngineCommand
{
    protected IOrderRiskEngineInternal OrderRiskEngine { get; }

    protected OrderCommand OrderCommand { get; }

    protected IUserProfilesRepository UserProfiles
        => OrderRiskEngine.UserProfiles;

    protected ISymbolSpecificationProvider SymbolSpecificationProvider
        => OrderRiskEngine.SymbolSpecificationProvider;

    protected ILastPriceCacheProvider LastPriceCacheProvider
        => OrderRiskEngine.LastPriceCacheProvider;

    protected IFeeCalculationService FeeCalculationService
        => OrderRiskEngine.FeeCalculationService;

    protected RiskEngineCommand(
        IOrderRiskEngineInternal orderRiskEngine,
        OrderCommand command)
    {
        OrderCommand = command;
        OrderRiskEngine = orderRiskEngine;
    }

    public abstract CommandResultCode Execute();
}


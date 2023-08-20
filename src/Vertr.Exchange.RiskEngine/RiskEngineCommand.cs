using Vertr.Exchange.Common.Enums;
using Vertr.Exchange.Common;
using Vertr.Exchange.RiskEngine.Abstractions;

namespace Vertr.Exchange.RiskEngine;

internal abstract class RiskEngineCommand
{
    protected IOrderRiskEngineInternal OrderRiskEngine { get; }

    protected OrderCommand OrderCommand { get; }

    protected IUserProfileService UserProfileService
        => OrderRiskEngine.UserProfileService;

    protected ISymbolSpecificationProvider SymbolSpecificationProvider
        => OrderRiskEngine.SymbolSpecificationProvider;

    protected ILastPriceCacheProvider LastPriceCacheProvider
        => OrderRiskEngine.LastPriceCacheProvider;

    protected RiskEngineCommand(
        IOrderRiskEngineInternal orderRiskEngine,
        OrderCommand command)
    {
        OrderCommand = command;
        OrderRiskEngine = orderRiskEngine;
    }

    public abstract CommandResultCode Execute();
}


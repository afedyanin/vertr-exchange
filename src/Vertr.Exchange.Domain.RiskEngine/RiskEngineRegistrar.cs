using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Domain.Common.Abstractions;
using Vertr.Exchange.Domain.RiskEngine.Symbols;

namespace Vertr.Exchange.Domain.RiskEngine;

public static class RiskEngineRegistrar
{
    public static IServiceCollection AddRiskEngine(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISymbolSpecificationProvider, SymbolSpecificationProvider>();
        serviceCollection.AddSingleton<IOrderRiskEngine, OrderRiskEngine>();
        return serviceCollection;
    }
}

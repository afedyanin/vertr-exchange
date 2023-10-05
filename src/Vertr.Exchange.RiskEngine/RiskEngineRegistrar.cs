using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.RiskEngine.Symbols;

namespace Vertr.Exchange.RiskEngine;

public static class RiskEngineRegistrar
{
    public static IServiceCollection AddRiskEngine(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISymbolSpecificationProvider, SymbolSpecificationProvider>();
        serviceCollection.AddSingleton<IOrderRiskEngine, OrderRiskEngine>();
        return serviceCollection;
    }
}

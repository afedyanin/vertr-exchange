using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.RiskEngine.Symbols;

namespace Vertr.Exchange.RiskEngine;

public static class OrderRiskEngineRegistrar
{
    public static IServiceCollection AddOrderRiskEngine(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISymbolSpecificationProvider, SymbolSpecificationProvider>();
        serviceCollection.AddSingleton<IOrderRiskEngine, OrderRiskEngine>();
        return serviceCollection;
    }
}

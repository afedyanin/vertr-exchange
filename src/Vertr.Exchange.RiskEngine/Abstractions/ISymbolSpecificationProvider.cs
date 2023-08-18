using Vertr.Exchange.Common.Symbols;

namespace Vertr.Exchange.RiskEngine.Abstractions;

public interface ISymbolSpecificationProvider
{
    bool AddSymbol(CoreSymbolSpecification symbolSpecification);

    CoreSymbolSpecification? GetSymbolSpecification(int symbol);

    void RegisterSymbol(int symbol, CoreSymbolSpecification spec);

    public void Reset();
}
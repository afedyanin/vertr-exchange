using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.RiskEngine.Symbols;

internal interface ISymbolSpecificationProvider
{
    bool AddSymbol(ISymbolSpecification symbolSpecification);

    ISymbolSpecification? GetSymbolSpecification(int symbol);

    void AddSymbols(ISymbolSpecification[] symbols);

    void RegisterSymbol(int symbol, ISymbolSpecification spec);

    public void Reset();
}

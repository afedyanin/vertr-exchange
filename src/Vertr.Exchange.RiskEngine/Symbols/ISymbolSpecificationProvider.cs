using Vertr.Exchange.Common;

namespace Vertr.Exchange.RiskEngine.Symbols;

internal interface ISymbolSpecificationProvider
{
    bool AddSymbol(SymbolSpecification symbolSpecification);

    SymbolSpecification? GetSymbolSpecification(int symbol);

    void AddSymbols(SymbolSpecification[] symbols);

    void RegisterSymbol(int symbol, SymbolSpecification spec);

    public void Reset();
}

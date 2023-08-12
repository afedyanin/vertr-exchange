using Vertr.Exchange.Domain.Symbols;

namespace Vertr.Exchange.Domain.Abstractions;

public interface ISymbolSpecificationProvider
{
    bool AddSymbol(CoreSymbolSpecification symbolSpecification);

    CoreSymbolSpecification? GetSymbolSpecification(int symbol);

    void RegisterSymbol(int symbol, CoreSymbolSpecification spec);

    public void Reset();
}

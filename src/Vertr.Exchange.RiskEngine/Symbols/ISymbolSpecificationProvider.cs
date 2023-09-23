using Vertr.Exchange.Common;
using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.RiskEngine.Symbols;

internal interface ISymbolSpecificationProvider
{
    SymbolSpecification? GetSymbol(int symbol);

    CommandResultCode AddSymbols(SymbolSpecification[] symbols);

    public void Reset();
}

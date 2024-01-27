using Vertr.Exchange.Domain.Common;
using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Domain.RiskEngine.Symbols;

internal interface ISymbolSpecificationProvider
{
    SymbolSpecification? GetSymbol(int symbol);

    CommandResultCode AddSymbols(SymbolSpecification[] symbols);

    public void Reset();
}

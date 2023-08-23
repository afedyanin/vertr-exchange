using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common.Symbols;

public class CoreSymbolSpecification
{
    public int SymbolId { get; set; }

    public SymbolType Type { get; set; }

    public int QuoteCurrency { get; } // quote/counter currency (OR futures contract currency)
}

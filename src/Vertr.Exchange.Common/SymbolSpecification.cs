using Vertr.Exchange.Common.Enums;

namespace Vertr.Exchange.Common;

public class SymbolSpecification
{
    public int SymbolId { get; set; }

    public SymbolType Type { get; set; }

    public int Currency { get; set; }
}



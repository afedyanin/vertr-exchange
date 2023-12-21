using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Common;

public class SymbolSpecification
{
    public int SymbolId { get; set; }

    public SymbolType Type { get; set; }

    public int Currency { get; set; }
}



using Vertr.Exchange.Domain.Common.Enums;

namespace Vertr.Exchange.Domain.Common;

public class SymbolSpecification
{
    public int SymbolId { get; set; }

    public SymbolType Type { get; set; }

    public int Currency { get; set; }
}



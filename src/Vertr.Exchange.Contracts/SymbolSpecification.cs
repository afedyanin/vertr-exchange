using Vertr.Exchange.Shared.Enums;

namespace Vertr.Exchange.Contracts;

public record SymbolSpecification
{
    public int SymbolId { get; set; }

    public SymbolType Type { get; set; }

    public int Currency { get; set; }
}

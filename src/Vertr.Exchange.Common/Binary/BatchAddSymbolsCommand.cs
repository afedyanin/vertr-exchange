using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Common.Binary;

public sealed class BatchAddSymbolsCommand : IBinaryCommand
{
    public SymbolSpecification[] Symbols { get; set; } =
        Array.Empty<SymbolSpecification>();
}

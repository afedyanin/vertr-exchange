using Vertr.Exchange.Common.Abstractions;
using Vertr.Exchange.Common.Symbols;

namespace Vertr.Exchange.Common.Binary;

public sealed class BatchAddSymbolsCommand : IBinaryCommand
{
    public CoreSymbolSpecification[] Symbols { get; set; } =
        Array.Empty<CoreSymbolSpecification>();
}

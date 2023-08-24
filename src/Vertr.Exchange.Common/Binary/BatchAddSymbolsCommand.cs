using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Common.Binary;

public sealed class BatchAddSymbolsCommand : IBinaryCommand
{
    public ISymbolSpecification[] Symbols { get; set; } =
        Array.Empty<ISymbolSpecification>();
}

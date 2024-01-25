using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Domain.Common.Binary.Commands;

public sealed class BatchAddSymbolsCommand : IBinaryCommand
{
    public SymbolSpecification[] Symbols { get; set; } =
        Array.Empty<SymbolSpecification>();
}

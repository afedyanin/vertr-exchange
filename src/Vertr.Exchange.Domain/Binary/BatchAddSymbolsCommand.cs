using Vertr.Exchange.Domain.Symbols;

namespace Vertr.Exchange.Domain.Binary;

internal sealed class BatchAddSymbolsCommand : BinaryCommand
{
    public CoreSymbolSpecification[] Symbols { get; set; } = Array.Empty<CoreSymbolSpecification>();
}

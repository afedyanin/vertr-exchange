namespace Vertr.Exchange.Domain.Binary;

internal sealed class BatchAddSymbolsCommand : BinaryCommand
{
    public int[] Symbols { get; set; } = Array.Empty<int>();
}

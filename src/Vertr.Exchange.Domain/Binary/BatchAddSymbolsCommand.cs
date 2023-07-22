namespace Vertr.Exchange.Domain.Binary;

internal class BatchAddSymbolsCommand : BinaryCommand
{
    public int[] Symbols { get; set; } = Array.Empty<int>();
}

namespace Vertr.Exchange.Domain.Binary;

internal sealed class BatchAddAccountsCommand : BinaryCommand
{
    // ExtKey = uud
    // IntKey = symbol
    // Value = balance
    public IDictionary<int, IDictionary<int, long>> Users { get; set; } = new Dictionary<int, IDictionary<int, long>>();
}

using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Common.Binary.Commands;

public sealed class BatchAddAccountsCommand : IBinaryCommand
{
    // ExtKey = uud
    // IntKey = symbol
    // Value = balance
    public IDictionary<int, IDictionary<int, long>> Users { get; set; } =
        new Dictionary<int, IDictionary<int, long>>();
}

using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Common.Binary.Commands;

public sealed class BatchAddAccountsCommand : IBinaryCommand
{
    // LongKey = uud
    // IntKey = currency
    // DecimalValue = balance
    public IDictionary<long, IDictionary<int, decimal>> Users { get; set; } =
        new Dictionary<long, IDictionary<int, decimal>>();
}

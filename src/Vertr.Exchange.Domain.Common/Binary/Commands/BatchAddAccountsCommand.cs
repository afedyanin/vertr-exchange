using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Domain.Common.Binary.Commands;

public sealed class BatchAddAccountsCommand : IBinaryCommand
{
    // LongKey = uud
    // IntKey = currency
    // DecimalValue = balance
    public IDictionary<long, IDictionary<int, decimal>> Users { get; set; } =
        new Dictionary<long, IDictionary<int, decimal>>();
}

using Google.Protobuf.Collections;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Server.Extensions;

internal static class UserAccountExtensions
{
    public static IDictionary<long, IDictionary<int, decimal>> ToDomain(this IEnumerable<UserAccount> accounts)
    {
        var res = new Dictionary<long, IDictionary<int, decimal>>();

        foreach (var account in accounts)
        {
            var key = account.UserId;
            var val = account.Balances.ToDomain();

            res.TryAdd(key, val);
        }

        return res;
    }

    private static Dictionary<int, decimal> ToDomain(this MapField<int, DecimalValue> balances)
    {
        var res = new Dictionary<int, decimal>();

        foreach (var balance in balances)
        {
            res.Add(balance.Key, balance.Value);
        }

        return res;
    }
}

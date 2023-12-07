using Vertr.Exchange.Contracts;

namespace Vertr.Exchange.Server.Extensions;

internal static class UserAccountExtensions
{
    public static IDictionary<long, IDictionary<int, decimal>> ToDomain(this IEnumerable<UserAccount> accounts)
    {
        var res = new Dictionary<long, IDictionary<int, decimal>>();

        foreach (var account in accounts)
        {
            var key = account.UserId;
            var val = account.Balances;

            res.TryAdd(key, val);
        }

        return res;
    }
}

using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.ApiClient.Extensions;
public static class BalanceExtensions
{
    public static Balance? GetByCurrencyId(this IEnumerable<Balance> balances, int id)
        => balances.FirstOrDefault(x => x.Currency.Id == id);

    public static Balance? GetByCurrency(this IEnumerable<Balance> balances, Currency currency)
        => balances.GetByCurrencyId(currency.Id);

    public static Balance? GetByCurrencyCode(this IEnumerable<Balance> balances, string code)
        => balances.FirstOrDefault(x => x.Currency.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
}

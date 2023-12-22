using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.ApiClient.Extensions;

public static class CurrencyExtensions
{
    public static Currency? GetById(this IEnumerable<Currency> currencies, int id)
        => currencies.FirstOrDefault(x => x.Id == id);

    public static Currency? GetByCode(this IEnumerable<Currency> currencies, string code)
        => currencies.FirstOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
}

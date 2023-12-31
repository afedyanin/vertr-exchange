using Vertr.Exchange.Contracts;
using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.ApiClient.Extensions;
public static class SymbolExtensions
{
    public static SymbolSpecification GetSpecification(this Symbol symbol)
    => new SymbolSpecification
    {
        Currency = symbol.Currency.Id,
        SymbolId = symbol.Id,
        Type = symbol.SymbolType
    };

    public static Symbol? GetById(this IEnumerable<Symbol> symbols, int id)
        => symbols.FirstOrDefault(x => x.Id == id);

    public static Symbol? GetByCode(this IEnumerable<Symbol> symbols, string code)
        => symbols.FirstOrDefault(s => s.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
}

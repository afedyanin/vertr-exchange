using Vertr.Exchange.Contracts;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.ConsoleApp.StaticData;
public record Symbol(int Id, Currency Currency, string Code, string Name, SymbolType SymbolType);


internal static class Symbols
{
    public static readonly Symbol MSFT = new Symbol(100, Currencies.USD, "MSFT", "Microsoft", SymbolType.EQUITY);
    public static readonly Symbol AAPL = new Symbol(200, Currencies.USD, "AAPL", "Apple", SymbolType.EQUITY);
    public static readonly Symbol GOOG = new Symbol(300, Currencies.USD, "GOOG", "Google", SymbolType.EQUITY);

    public static readonly Symbol SBER = new Symbol(1100, Currencies.RUB, "SBER", "Сбербанк", SymbolType.EQUITY);
    public static readonly Symbol GMKN = new Symbol(1200, Currencies.RUB, "GMKN", "ГМК Норникель", SymbolType.EQUITY);
    public static readonly Symbol ROSN = new Symbol(1200, Currencies.RUB, "ROSN", "Роснефть", SymbolType.EQUITY);

    public static readonly Symbol[] All = [MSFT, AAPL, GOOG, SBER, GMKN, ROSN];

    public static SymbolSpecification GetSpecification(this Symbol symbol)
        => new SymbolSpecification
        {
            Currency = symbol.Currency.Id,
            SymbolId = symbol.Id,
            Type = symbol.SymbolType
        };

    public static Symbol? GetById(int id) => All.FirstOrDefault(x => x.Id == id);

    public static Symbol? GetByCode(string code) => All.FirstOrDefault(s => s.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
}

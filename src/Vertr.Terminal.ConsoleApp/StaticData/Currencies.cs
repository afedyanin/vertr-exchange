namespace Vertr.Terminal.ConsoleApp.StaticData;

public record Currency(int Id, string Code, string Name);

internal static class Currencies
{
    public static readonly Currency USD = new Currency(100, "USD", "US Dollar");
    public static readonly Currency EUR = new Currency(300, "EUR", "Euro");
    public static readonly Currency RUB = new Currency(200, "RUB", "Russian Rub");

    public static readonly Currency[] All = [USD, EUR, RUB];

    public static Currency? GetById(int id) => All.FirstOrDefault(x => x.Id == id);

    public static Currency? GetByCode(string code) => All.FirstOrDefault(x => x.Code.Equals(code, StringComparison.OrdinalIgnoreCase));
}

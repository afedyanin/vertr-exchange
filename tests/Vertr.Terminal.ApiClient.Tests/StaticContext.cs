using Vertr.Exchange.Shared.Enums;
using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.ApiClient.Tests;
internal static class StaticContext
{
    public static class Currencies
    {
        public static readonly Currency USD = new Currency(100, "USD", "US Dollar");
        public static readonly Currency EUR = new Currency(300, "EUR", "Euro");
        public static readonly Currency RUB = new Currency(200, "RUB", "Russian Rub");

        public static readonly Currency[] All = [USD, EUR, RUB];
    }

    public static class Symbols
    {
        public static readonly Symbol MSFT = new Symbol(100, Currencies.USD, "MSFT", "Microsoft", SymbolType.EQUITY);
        public static readonly Symbol AAPL = new Symbol(200, Currencies.USD, "AAPL", "Apple", SymbolType.EQUITY);
        public static readonly Symbol GOOG = new Symbol(300, Currencies.USD, "GOOG", "Google", SymbolType.EQUITY);

        public static readonly Symbol SBER = new Symbol(1100, Currencies.RUB, "SBER", "Сбербанк", SymbolType.EQUITY);
        public static readonly Symbol GMKN = new Symbol(1200, Currencies.RUB, "GMKN", "ГМК Норникель", SymbolType.EQUITY);
        public static readonly Symbol ROSN = new Symbol(1200, Currencies.RUB, "ROSN", "Роснефть", SymbolType.EQUITY);

        public static readonly Symbol[] All = [MSFT, AAPL, GOOG, SBER, GMKN, ROSN];
    }

    public static class Users
    {
        public static readonly User Alice = new User(10, "Alice");
        public static readonly User Bob = new User(20, "Bob");

        public static readonly User[] All = [Alice, Bob];
    }

    public static class UserAccounts
    {
        public static readonly UserAccount BobAccount =
            new UserAccount(
                Users.Bob,
                [
                    new Balance(Currencies.USD, 100_000m),
                    new Balance(Currencies.EUR, 50_000m),
                    new Balance(Currencies.RUB, 20_000_000m)
                ]);

        public static readonly UserAccount AliceAccount =
            new UserAccount(
                Users.Alice,
                [
                    new Balance(Currencies.USD, 100_000m),
                    new Balance(Currencies.EUR, 50_000m),
                    new Balance(Currencies.RUB, 20_000_000m)
                ]);
    }
}

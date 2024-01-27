using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Enums;

namespace Vertr.Exchange.SignalRClient.ConsoleApp;

public record User(long Id, string Name);

public record Currency(int Id, string Code, string Name);

public record Symbol(int Id, Currency Currency, string Code, string Name, SymbolType SymbolType);


internal static class StaticContext
{
    public static class Currencies
    {
        public static readonly Currency USD = new Currency(100, "USD", "US Dollar");
        public static readonly Currency EUR = new Currency(300, "EUR", "Euro");
    }

    public static class Symbols
    {
        public static readonly Symbol MSFT = new Symbol(100, Currencies.USD, "MSFT", "Microsoft", SymbolType.EQUITY);
        public static readonly Symbol AAPL = new Symbol(200, Currencies.USD, "AAPL", "Apple", SymbolType.EQUITY);
        public static readonly Symbol GOOG = new Symbol(300, Currencies.USD, "GOOG", "Google", SymbolType.EQUITY);

        public static readonly SymbolSpecification[] AllSymbolSpecs =
        [
            new SymbolSpecification()
            {
                Currency = MSFT.Currency.Id,
                SymbolId = MSFT.Id,
                Type = MSFT.SymbolType
            },
            new SymbolSpecification()
            {
                Currency = AAPL.Currency.Id,
                SymbolId = AAPL.Id,
                Type = AAPL.SymbolType
            },
            new SymbolSpecification()
            {
                Currency = GOOG.Currency.Id,
                SymbolId = GOOG.Id,
                Type = GOOG.SymbolType
            },
        ];
    }

    public static class Users
    {
        public static readonly User Alice = new User(10, "Alice");
        public static readonly User Bob = new User(20, "Bob");
    }

    public static class UserAccounts
    {
        public static UserAccount[] All = [
            new UserAccount
            {
                UserId = Users.Alice.Id,
                Balances = new Dictionary<int, decimal>()
                {
                    { Currencies.USD.Id, 100_000m},
                    { Currencies.EUR.Id, 50_000m},
                }
            },
            new UserAccount
            {
                UserId = Users.Bob.Id,
                Balances = new Dictionary<int, decimal>()
                {
                    { Currencies.USD.Id, 100_000m},
                    { Currencies.EUR.Id, 50_000m},
                }
            },
        ];
    }
}

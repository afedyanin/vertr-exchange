namespace Vertr.Exchange.Client.Host.StaticData;

public record Balance(Currency Currency, decimal Amount);

public record UserAccount(User User, Balance[] Balances);

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

    public static Protos.UserAccount ToProto(this UserAccount userAccount)
    {
        var account = new Protos.UserAccount()
        {
            UserId = userAccount.User.Id,
        };

        foreach (var bal in userAccount.Balances)
        {
            account.Balances.Add(bal.Currency.Id, bal.Amount);
        }

        return account;
    }
}

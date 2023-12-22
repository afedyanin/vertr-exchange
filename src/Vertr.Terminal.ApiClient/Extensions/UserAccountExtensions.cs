using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.ApiClient.Extensions;

public static class UserAccountExtensions
{
    public static Exchange.Contracts.UserAccount ToDto(this UserAccount userAccount)
    {
        var account = new Exchange.Contracts.UserAccount()
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

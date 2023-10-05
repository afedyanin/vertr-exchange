using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Accounts;
public static class AccountsRegistrar
{
    public static IServiceCollection AddAccounts(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUserProfileProvider, UserProfileProvider>();
        return serviceCollection;
    }
}

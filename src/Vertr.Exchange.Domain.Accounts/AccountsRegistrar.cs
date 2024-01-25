using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Domain.Accounts;
public static class AccountsRegistrar
{
    public static IServiceCollection AddAccounts(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUserProfileProvider, UserProfileProvider>();
        return serviceCollection;
    }
}

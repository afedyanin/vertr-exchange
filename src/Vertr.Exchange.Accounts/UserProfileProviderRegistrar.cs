using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Accounts;
public static class UserProfileProviderRegistrar
{
    public static IServiceCollection AddUserProfileProvider(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUserProfileProvider, UserProfileProvider>();
        return serviceCollection;
    }
}

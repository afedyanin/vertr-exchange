using Microsoft.Extensions.DependencyInjection;
using Vertr.Exchange.Adapters.SignalR.MessageHandlers;
using Vertr.Exchange.Application.Messages;

namespace Vertr.Exchange.Adapters.SignalR;

public static class SignalrAdapterRegistrar
{
    public static IServiceCollection AddExchangeSignalrAdapter(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ObservableMessageHandler>();
        serviceCollection.AddSingleton<IObservableMessageHandler>(
            x => x.GetRequiredService<ObservableMessageHandler>());
        serviceCollection.AddSingleton<IMessageHandler>(
            x => x.GetRequiredService<ObservableMessageHandler>());

        return serviceCollection;
    }
}

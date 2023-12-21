using Microsoft.AspNetCore.SignalR.Client;

namespace Vertr.Terminal.ExchangeClient.Providers;

internal interface IHubConnectionProvider
{
    Task<HubConnection> GetConnection();
}

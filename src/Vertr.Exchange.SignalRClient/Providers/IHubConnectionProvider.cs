using Microsoft.AspNetCore.SignalR.Client;

namespace Vertr.Exchange.SignalRClient.Providers;

internal interface IHubConnectionProvider
{
    Task<HubConnection> GetConnection();
}

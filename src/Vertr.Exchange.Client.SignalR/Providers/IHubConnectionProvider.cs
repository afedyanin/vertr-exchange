using Microsoft.AspNetCore.SignalR.Client;

namespace Vertr.Exchange.Client.SignalR.Providers;

internal interface IHubConnectionProvider
{
    Task<HubConnection> GetConnection();
}

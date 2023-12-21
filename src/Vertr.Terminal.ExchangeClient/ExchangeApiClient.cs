using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Common.Messages;
using Vertr.Terminal.ExchangeClient.Awaiting;
using Vertr.Terminal.ExchangeClient.Providers;

namespace Vertr.Terminal.ExchangeClient;

internal class ExchangeApiClient(
    IHubConnectionProvider connectionProvider,
    ICommandAwaitingService commandAwaitingService) : IExchangeApiClient
{
    private readonly IHubConnectionProvider _connectionProvider = connectionProvider;
    private readonly ICommandAwaitingService _commandAwaitingService = commandAwaitingService;

    protected async Task<ApiCommandResult> InvokeHubMethod(
        string methodName,
        object request,
        CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeCoreAsync<long>(methodName, [request], cancellationToken);
        var result = await _commandAwaitingService.Register(commandId);
        return result.CommandResult;
    }

    protected async Task<ApiCommandResult> InvokeHubMethod(
        string methodName,
        CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeAsync<long>(methodName, cancellationToken);
        var result = await _commandAwaitingService.Register(commandId);
        return result.CommandResult;
    }

}

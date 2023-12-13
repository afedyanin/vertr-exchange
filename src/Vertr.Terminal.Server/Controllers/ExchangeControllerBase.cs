using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Terminal.Server.Awaiting;
using Vertr.Terminal.Server.Providers;

namespace Vertr.Terminal.Server.Controllers;

public abstract class ExchangeControllerBase(
    HubConnectionProvider connectionProvider,
    ICommandAwaitingService commandAwaitingService
    ) : ControllerBase
{
    private readonly ICommandAwaitingService _commandAwaitingService = commandAwaitingService;
    private readonly HubConnectionProvider _connectionProvider = connectionProvider;

    protected async Task<ApiCommandResult> InvokeHubMethod(string methodName, object request, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeCoreAsync<long>(methodName, [request], cancellationToken);
        var result = await _commandAwaitingService.Register(commandId);
        return result.CommandResult;
    }

    protected async Task<ApiCommandResult> InvokeHubMethod(string methodName, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeAsync<long>(methodName, cancellationToken);
        var result = await _commandAwaitingService.Register(commandId);
        return result.CommandResult;
    }

}

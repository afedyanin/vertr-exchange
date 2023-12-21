using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.ExchangeClient.Awaiting;
using Vertr.Terminal.ExchangeClient.Providers;

namespace Vertr.Terminal.ExchangeClient;

internal class ExchangeApiClient(
    IHubConnectionProvider connectionProvider,
    ICommandAwaitingService commandAwaitingService) : IExchangeApiClient
{
    private readonly IHubConnectionProvider _connectionProvider = connectionProvider;
    private readonly ICommandAwaitingService _commandAwaitingService = commandAwaitingService;

    public async Task<ApiCommandResult> AddAccounts(AddAccountsRequest request)
    {
        var res = await InvokeHubMethod("AddAccounts", request);
        return res;
    }

    public async Task<ApiCommandResult> AddSymbols(AddSymbolsRequest request)
    {
        var res = await InvokeHubMethod("AddSymbols", request);
        return res;
    }

    public async Task<ApiCommandResult> Nop()
    {
        var res = await InvokeHubMethod("Nop");
        return res;
    }

    public async Task<ApiCommandResult> PlaceOrder(PlaceOrderRequest request)
    {
        var res = await InvokeHubMethod("PlaceOrder", request);
        return res;
    }

    public async Task<ApiCommandResult> Reset()
    {
        var res = await InvokeHubMethod("Reset");
        return res;
    }

    private async Task<ApiCommandResult> InvokeHubMethod(
        string methodName,
        object request,
        CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeCoreAsync<long>(methodName, [request], cancellationToken);
        var result = await _commandAwaitingService.Register(commandId);
        return result.CommandResult;
    }

    private async Task<ApiCommandResult> InvokeHubMethod(
        string methodName,
        CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeAsync<long>(methodName, cancellationToken);
        var result = await _commandAwaitingService.Register(commandId);
        return result.CommandResult;
    }

}

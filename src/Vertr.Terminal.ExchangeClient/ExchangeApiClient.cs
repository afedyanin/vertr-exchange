using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.Domain.Abstractions;
using Vertr.Terminal.ExchangeClient.Awaiting;
using Vertr.Terminal.ExchangeClient.Providers;

namespace Vertr.Terminal.ExchangeClient;

internal sealed class ExchangeApiClient(
    IHubConnectionProvider connectionProvider,
    ICommandAwaitingService commandAwaitingService) : IExchangeApiClient
{
    private readonly IHubConnectionProvider _connectionProvider = connectionProvider;
    private readonly ICommandAwaitingService _commandAwaitingService = commandAwaitingService;

    public Task<ApiCommandResult> AddAccounts(AddAccountsRequest request)
        => InvokeHubMethod("AddAccounts", request);

    public Task<ApiCommandResult> AddSymbols(AddSymbolsRequest request)
        => InvokeHubMethod("AddSymbols", request);

    public Task<ApiCommandResult> CancelOrder(CancelOrderRequest request)
        => InvokeHubMethod("CancelOrder", request);

    public Task<ApiCommandResult> GetSingleUserReport(UserRequest request)
        => InvokeHubMethod("GetSingleUserReport", request);

    public Task<ApiCommandResult> MoveOrder(MoveOrderRequest request)
        => InvokeHubMethod("MoveOrder", request);

    public Task<ApiCommandResult> Nop()
        => InvokeHubMethod("Nop");

    public async Task<long> GetNextOrderId()
    {
        var connection = await _connectionProvider.GetConnection();
        var nextOrderId = await connection.InvokeAsync<long>("GetNextOrderId");
        return nextOrderId;
    }

    public Task<ApiCommandResult> PlaceOrder(PlaceOrderRequest request, long? orderId = null)
    {
        var res = orderId.HasValue ?
            InvokeHubMethod("PlaceOrder", [request, orderId.Value]) :
            InvokeHubMethod("PlaceOrder", request);

        return res;
    }

    public Task<ApiCommandResult> ReduceOrder(ReduceOrderRequest request)
        => InvokeHubMethod("ReduceOrder", request);

    public Task<ApiCommandResult> Reset()
        => InvokeHubMethod("Reset");

    private Task<ApiCommandResult> InvokeHubMethod(
        string methodName,
        object request,
        CancellationToken cancellationToken = default)
    {
        return InvokeHubMethod(methodName, [request], cancellationToken);
    }

    private async Task<ApiCommandResult> InvokeHubMethod(
        string methodName,
        object[] requestItems,
        CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeCoreAsync<long>(methodName, requestItems, cancellationToken);
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Client.Host.Awaiting;
using Vertr.Exchange.Client.Host.Providers;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Exchange.Client.Host.Controllers;
[Route("api/exchange")]
[ApiController]
public class ExchangeController(
    HubConnectionProvider connectionProvider,
    ICommandAwaitingService commandAwaitingService,
    ILogger<ExchangeController> logger) : ControllerBase
{
    private readonly ILogger<ExchangeController> _logger = logger;
    private readonly HubConnectionProvider _connectionProvider = connectionProvider;
    private readonly ICommandAwaitingService _commandAwaitingService = commandAwaitingService;

    [HttpPost("symbols")]
    public async Task<IActionResult> AddSymbols(AddSymbolsRequest request)
    {
        var res = await InvokeHubMethod("AddSymbols", request);
        return Ok(res);
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> AddAccounts(AddAccountsRequest request)
    {
        var res = await InvokeHubMethod("AddAccounts", request);
        return Ok(res);
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset()
    {
        var res = await InvokeHubMethod("Reset");
        return Ok(res);
    }

    [HttpPost("nop")]
    public async Task<IActionResult> Nop()
    {
        var res = await InvokeHubMethod("Nop");
        return Ok(res);
    }

    [HttpPost("place-order")]
    public async Task<IActionResult> PlaceOrder(PlaceOrderRequest request)
    {
        var res = await InvokeHubMethod("PlaceOrder", request);
        return Ok(res);
    }

    private async Task<ApiCommandResult> InvokeHubMethod(string methodName, object request, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeCoreAsync<long>(methodName, [request], cancellationToken);
        var result = await _commandAwaitingService.Register(commandId);
        return result.CommandResult;
    }

    private async Task<ApiCommandResult> InvokeHubMethod(string methodName, CancellationToken cancellationToken = default)
    {
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeAsync<long>(methodName, cancellationToken);
        var result = await _commandAwaitingService.Register(commandId);
        return result.CommandResult;
    }
}

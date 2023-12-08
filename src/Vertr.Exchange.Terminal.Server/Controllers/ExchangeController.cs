using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Exchange.Terminal.Server.Awaiting;
using Vertr.Exchange.Terminal.Server.Providers;
using Vertr.Exchange.Terminal.Server.Repositories;

namespace Vertr.Exchange.Terminal.Server.Controllers;

[Route("api/exchange")]
[ApiController]
public class ExchangeController(
    HubConnectionProvider connectionProvider,
    ICommandAwaitingService commandAwaitingService,
    IOrderBookRepository orderBookRepository,
    ILogger<ExchangeController> logger) : ControllerBase
{
    private readonly ILogger<ExchangeController> _logger = logger;
    private readonly HubConnectionProvider _connectionProvider = connectionProvider;
    private readonly ICommandAwaitingService _commandAwaitingService = commandAwaitingService;
    private readonly IOrderBookRepository _orderBookRepository = orderBookRepository;

    [HttpPost("symbols")]
    public async Task<IActionResult> AddSymbols(AddSymbolsRequest request)
    {
        _logger.LogInformation("AddSymbols request received");

        var res = await InvokeHubMethod("AddSymbols", request);

        _logger.LogInformation("AddSymbols request completed {res}", res);

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

    [HttpGet("order-books/{symbolId:int}")]
    public async Task<IActionResult> GetOrderBook(int symbolId)
    {
        var ob = await _orderBookRepository.Get(symbolId);

        if (ob is null)
        {
            return NotFound();
        }

        return Ok(ob);
    }

    [HttpGet("order-books")]
    public async Task<IActionResult> GetOrderBooks()
    {
        var ob = await _orderBookRepository.GetList();
        return Ok(ob);
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

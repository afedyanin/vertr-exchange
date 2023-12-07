using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Client.Host.Awaiting;
using Vertr.Exchange.Client.Host.Providers;
using Vertr.Exchange.Client.Host.StaticData;
using Vertr.Exchange.Contracts.Enums;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Exchange.Client.Host.Controllers;
[Route("api/admin")]
[ApiController]
public class AdminController(
    HubConnectionProvider connectionProvider,
    ICommandAwaitingService commandAwaitingService,
    ILogger<AdminController> logger) : ControllerBase
{
    private readonly ILogger<AdminController> _logger = logger;
    private readonly HubConnectionProvider _connectionProvider = connectionProvider;
    private readonly ICommandAwaitingService _commandAwaitingService = commandAwaitingService;

    [HttpPost("symbols")]
    public async Task<IActionResult> AddSymbols()
    {
        _logger.LogInformation($"Setup Symbols...");
        var req = CreateAddSymbolsRequest();
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeCoreAsync<long>("AddSymbols", new object[] { req });
        var result = await _commandAwaitingService.Register(commandId);
        return Ok(result.CommandResult);
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> AddAccounts()
    {
        _logger.LogInformation($"Setup Accounts...");
        var req = CreateAddAccountsRequest();
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeCoreAsync<long>("AddAccounts", new object[] { req });
        var result = await _commandAwaitingService.Register(commandId);
        return Ok(result.CommandResult);
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset()
    {
        _logger.LogInformation($"Reset exchange...");
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeAsync<long>("Reset");
        var result = await _commandAwaitingService.Register(commandId);
        return Ok(result.CommandResult);
    }

    [HttpPost("nop")]
    public async Task<IActionResult> Nop()
    {
        _logger.LogInformation($"NOP...");
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeAsync<long>("Nop");
        var result = await _commandAwaitingService.Register(commandId);
        return Ok(result.CommandResult);
    }

    [HttpPost("place-order/{user}/{symbol}")]
    public async Task<IActionResult> PlaceOrder(string symbol, string user, decimal price, long size)
    {
        if (size == 0L)
        {
            return BadRequest("Size must be provided.");
        }

        var usr = Users.GetByName(user);
        if (usr == null)
        {
            return NotFound("User not found.");
        }

        var sym = Symbols.GetByCode(symbol);
        if (sym == null)
        {
            return NotFound("Symbol not found.");
        }

        _logger.LogInformation($"Place Order started ...");
        var req = CreatePlaceOrderRequest(usr, sym, price, size);
        var connection = await _connectionProvider.GetConnection();
        var commandId = await connection.InvokeCoreAsync<long>("PlaceOrder", new object[] { req });
        var result = await _commandAwaitingService.Register(commandId);
        return Ok(result.CommandResult);
    }

    private static AddSymbolsRequest CreateAddSymbolsRequest()
    {
        var req = new AddSymbolsRequest()
        {
            Symbols = Symbols.All.Select(s => s.GetSpecification()).ToArray(),
        };
        return req;
    }

    private static AddAccountsRequest CreateAddAccountsRequest()
    {
        var req = new AddAccountsRequest()
        {
            UserAccounts =
            [
                UserAccounts.AliceAccount.ToDto(),
                UserAccounts.BobAccount.ToDto()
            ],
        };

        return req;
    }

    private static PlaceOrderRequest CreatePlaceOrderRequest(
        User user,
        Symbol symbol,
        decimal price,
        long size)
    {
        var req = new PlaceOrderRequest()
        {
            UserId = user.Id,
            Symbol = symbol.Id,
            Price = price,
            Size = Math.Abs(size),
            Action = size > 0 ? OrderAction.BID : OrderAction.ASK,
            OrderType = price == decimal.Zero ? OrderType.IOC : OrderType.GTC,
        };

        return req;
    }
}

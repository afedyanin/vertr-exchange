using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Vertr.Exchange.Client.Host.Providers;
using Vertr.Exchange.Client.Host.StaticData;
using Vertr.Exchange.Protos;

namespace Vertr.Exchange.Client.Host.Controllers;
[Route("api/admin")]
[ApiController]
public class AdminController(
    HubConnectionProvider connectionProvider,
    ILogger<AdminController> logger) : ControllerBase
{
    private readonly ILogger<AdminController> _logger = logger;
    private readonly HubConnectionProvider _connectionProvider = connectionProvider;

    [HttpPost("symbols")]
    public async Task AddSymbols()
    {
        _logger.LogInformation($"Setup Symbols...");
        var req = CreateAddSymbolsRequest();
        var connection = await _connectionProvider.GetConnection();
        await connection.InvokeCoreAsync("AddSymbols", new object[] { req });
        _logger.LogInformation($"Symbol setup completed.");
    }

    [HttpPost("accounts")]
    public async Task AddAccounts()
    {
        _logger.LogInformation($"Setup Accounts...");
        var req = CreateAddAccountsRequest();
        var connection = await _connectionProvider.GetConnection();
        await connection.InvokeCoreAsync("AddAccounts", new object[] { req });
        _logger.LogInformation($"Accounts setup completed.");
    }

    [HttpPost("reset")]
    public async Task Reset()
    {
        _logger.LogInformation($"Reset exchange...");
        var connection = await _connectionProvider.GetConnection();
        await connection.InvokeAsync("Reset");
        _logger.LogInformation($"Reset completed.");
    }

    [HttpPost("nop")]
    public async Task Nop()
    {
        _logger.LogInformation($"NOP...");
        var connection = await _connectionProvider.GetConnection();
        await connection.InvokeAsync("Nop");
        _logger.LogInformation($"NOP completed.");
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
        await connection.InvokeCoreAsync("PlaceOrder", new object[] { req });
        _logger.LogInformation($"Place Order completed.");
        return Ok();
    }

    private static AddSymbolsRequest CreateAddSymbolsRequest()
    {
        var req = new AddSymbolsRequest();
        req.Symbols.Add(Symbols.All.Select(s => s.GetSpecification()));
        return req;
    }

    private static AddAccountsRequest CreateAddAccountsRequest()
    {
        var req = new AddAccountsRequest();
        req.UserAccounts.Add(UserAccounts.AliceAccount.ToProto());
        req.UserAccounts.Add(UserAccounts.BobAccount.ToProto());

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
            Action = size > 0 ? OrderAction.Bid : OrderAction.Ask,
            OrderType = price == decimal.Zero ? OrderType.Ioc : OrderType.Gtc,
        };

        return req;
    }
}

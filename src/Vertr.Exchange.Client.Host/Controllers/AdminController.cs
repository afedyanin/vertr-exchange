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
    public async Task Post()
    {
        _logger.LogInformation($"Setup Symbols...");
        var req = CreateAddSymbolsRequest();
        var connection = await _connectionProvider.GetConnection();
        await connection.InvokeCoreAsync("AddSymbols", new object[] { req });
        _logger.LogInformation($"Symbol setup completed.");
    }

    private static AddSymbolsRequest CreateAddSymbolsRequest()
    {
        var req = new AddSymbolsRequest();
        req.Symbols.Add(Symbols.All.Select(s => s.GetSpecification()));
        return req;
    }
}

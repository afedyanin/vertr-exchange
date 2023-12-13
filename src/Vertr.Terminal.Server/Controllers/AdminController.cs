using Microsoft.AspNetCore.Mvc;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Exchange.Shared.Enums;
using Vertr.Terminal.Server.Awaiting;
using Vertr.Terminal.Server.OrderManagement;
using Vertr.Terminal.Server.Providers;
using Vertr.Terminal.Server.Repositories;

namespace Vertr.Terminal.Server.Controllers;

[Route("admin")]
[ApiController]
public class AdminController(
    HubConnectionProvider connectionProvider,
    ICommandAwaitingService commandAwaitingService,
    IOrderBookSnapshotsRepository orderBookRepository,
    ITradeEventsRepository tradeEventsRepository,
    IOrderEventHandler orderEventHandler,
    ILogger<AdminController> logger)
    : ExchangeControllerBase(
        connectionProvider,
        commandAwaitingService)
{
    private readonly IOrderBookSnapshotsRepository _orderBookRepository = orderBookRepository;
    private readonly ITradeEventsRepository _tradeEventsRepository = tradeEventsRepository;
    private readonly IOrderEventHandler _orderEventHandler = orderEventHandler;
    private readonly ILogger<AdminController> _logger = logger;

    [HttpPost("add-symbols")]
    public async Task<IActionResult> AddSymbols(AddSymbolsRequest request)
    {
        _logger.LogInformation("AddSymbols request received");

        var res = await InvokeHubMethod("AddSymbols", request);

        _logger.LogInformation("AddSymbols request completed {res}", res);

        return Ok(res);
    }

    [HttpPost("add-accounts")]
    public async Task<IActionResult> AddAccounts(AddAccountsRequest request)
    {
        var res = await InvokeHubMethod("AddAccounts", request);
        return Ok(res);
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset()
    {
        var res = await InvokeHubMethod("Reset");

        if (res.ResultCode == CommandResultCode.SUCCESS)
        {
            await _orderBookRepository.Reset();
            await _tradeEventsRepository.Reset();
            await _orderEventHandler.Reset();
        }

        return Ok(res);
    }
}


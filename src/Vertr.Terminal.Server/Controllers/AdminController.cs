using Microsoft.AspNetCore.Mvc;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Exchange.Shared.Enums;
using Vertr.Terminal.ExchangeClient;
using Vertr.Terminal.Server.OrderManagement;
using Vertr.Terminal.Server.Repositories;

namespace Vertr.Terminal.Server.Controllers;

[Route("admin")]
[ApiController]
public class AdminController(
    IExchangeApiClient exchangeApiClient,
    IOrderBookSnapshotsRepository orderBookRepository,
    ITradeEventsRepository tradeEventsRepository,
    IOrderEventHandler orderEventHandler) : ControllerBase
{
    private readonly IOrderBookSnapshotsRepository _orderBookRepository = orderBookRepository;
    private readonly ITradeEventsRepository _tradeEventsRepository = tradeEventsRepository;
    private readonly IOrderEventHandler _orderEventHandler = orderEventHandler;
    private readonly IExchangeApiClient _exchangeApiClient = exchangeApiClient;

    [HttpPost("add-symbols")]
    public async Task<IActionResult> AddSymbols(AddSymbolsRequest request)
    {
        var res = await _exchangeApiClient.AddSymbols(request);
        return Ok(res);
    }

    [HttpPost("add-accounts")]
    public async Task<IActionResult> AddAccounts(AddAccountsRequest request)
    {
        var res = await _exchangeApiClient.AddAccounts(request);
        return Ok(res);
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset()
    {
        var res = await _exchangeApiClient.Reset();

        if (res.ResultCode == CommandResultCode.SUCCESS)
        {
            await _orderBookRepository.Reset();
            await _tradeEventsRepository.Reset();
            await _orderEventHandler.Reset();
        }

        return Ok(res);
    }
}


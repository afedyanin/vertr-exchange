using Microsoft.AspNetCore.Mvc;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.Server.Awaiting;
using Vertr.Terminal.Server.Providers;
using Vertr.Terminal.Server.Repositories;

namespace Vertr.Terminal.Server.Controllers;

[Route("queries")]
[ApiController]
public class QueriesController(
    HubConnectionProvider connectionProvider,
    ICommandAwaitingService commandAwaitingService,
    IOrderBookSnapshotsRepository orderBookRepository,
    ITradeEventsRepository tradeEventsRepository,
    IOrdersRepository ordersRepository)
    : ExchangeControllerBase(
        connectionProvider,
        commandAwaitingService)
{
    private readonly IOrderBookSnapshotsRepository _orderBookRepository = orderBookRepository;
    private readonly ITradeEventsRepository _tradeEventsRepository = tradeEventsRepository;
    private readonly IOrdersRepository _ordersRepository = ordersRepository;

    [HttpPost("user-report")]
    public async Task<IActionResult> GetSingleUserReport(UserRequest request)
    {
        var res = await InvokeHubMethod("GetSingleUserReport", request);
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

    [HttpGet("trades")]
    public async Task<IActionResult> GetTradeEvents()
    {
        var te = await _tradeEventsRepository.GetList();
        return Ok(te);
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _ordersRepository.GetList();
        return Ok(orders);
    }
}

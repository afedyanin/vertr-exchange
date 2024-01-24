using Microsoft.AspNetCore.Mvc;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.ApiClient.Contracts;
using Vertr.Terminal.Domain.Abstractions;
using Vertr.Terminal.Server.Extensions;

namespace Vertr.Terminal.Server.Controllers;

[Route("queries")]
[ApiController]
public class QueriesController(
    IExchangeApiClient exchangeApiClient,
    IOrderBookSnapshotsRepository orderBookRepository,
    IPortfolioRepository portfolioRepository,
    ITradeEventsRepository tradeEventsRepository,
    IOrderRepository ordersRepository,
    IMarketDataRepository marketDataRepository)
    : ControllerBase
{
    private readonly IOrderBookSnapshotsRepository _orderBookRepository = orderBookRepository;
    private readonly ITradeEventsRepository _tradeEventsRepository = tradeEventsRepository;
    private readonly IOrderRepository _ordersRepository = ordersRepository;
    private readonly IPortfolioRepository _portfolioRepository = portfolioRepository;
    private readonly IExchangeApiClient _exchangeApiClient = exchangeApiClient;
    private readonly IMarketDataRepository _marketDataRepository = marketDataRepository;

    [HttpPost("user-report")]
    public async Task<ActionResult<ApiCommandResult?>> GetSingleUserReport(UserRequest request)
    {
        var res = await _exchangeApiClient.GetSingleUserReport(request);
        return Ok(res);
    }

    [HttpGet("order-books/{symbolId:int}")]
    public async Task<ActionResult<OrderBook?>> GetOrderBook(int symbolId)
    {
        var ob = await _orderBookRepository.Get(symbolId);
        return Ok(ob);
    }

    [HttpGet("order-books")]
    public async Task<ActionResult<OrderBook[]>> GetOrderBooks()
    {
        var ob = await _orderBookRepository.GetList();
        return Ok(ob);
    }

    [HttpGet("trades")]
    public async Task<ActionResult<TradeEvent[]>> GetTradeEvents()
    {
        var te = await _tradeEventsRepository.GetList();
        return Ok(te);
    }

    [HttpGet("orders")]
    public async Task<ActionResult<OrderDto[]>> GetOrders()
    {
        var orders = await _ordersRepository.GetList();
        return Ok(orders.ToDto());
    }

    [HttpGet("portfolios")]
    public async Task<ActionResult<PortfolioDto[]>> GetPortfolios()
    {
        var portfolio = await _portfolioRepository.GetList();
        return Ok(portfolio.ToDto());
    }

    [HttpGet("market-data")]
    public async Task<ActionResult<MarketDataItemDto[]>> GetMarketDataSnapshot()
    {
        var items = await _marketDataRepository.GetSnapshot();
        return Ok(items.ToDto());
    }

    [HttpGet("market-data/{symbolId:int}")]
    public async Task<ActionResult<MarketDataItemDto?>> GetMarketData(int symbolId)
    {
        var item = await _marketDataRepository.GetBySymbolId(symbolId);
        if (item == null)
        {
            return Ok();
        }

        return Ok(item.ToDto());
    }

    [HttpGet("market-data/{symbolId:int}/history")]
    public async Task<ActionResult<MarketDataItemDto[]>> GetMerketDataHistory(int symbolId)
    {
        var items = await _marketDataRepository.GetHistory(symbolId);
        return Ok(items.ToDto());
    }
}

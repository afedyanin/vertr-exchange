using Refit;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.ApiClient;

public interface ITerminalApiClient
{
    [Post("/admin/add-symbols")]
    Task<ApiCommandResult?> AddSymbols([Body] AddSymbolsRequest request);

    [Post("/admin/add-accounts")]
    Task<ApiCommandResult?> AddAccounts([Body] AddAccountsRequest request);

    [Post("/admin/reset")]
    Task<ApiCommandResult?> Reset();

    [Post("/commands/nop")]
    Task<ApiCommandResult?> Nop();

    [Post("/commands/place-order")]
    Task<ApiCommandResult?> PlaceOrder([Body] PlaceOrderRequest request);

    [Post("/commands/cancel-order")]
    Task<ApiCommandResult?> CancelOrder([Body] CancelOrderRequest request);

    [Post("/commands/move-order")]
    Task<ApiCommandResult?> MoveOrder([Body] MoveOrderRequest request);

    [Post("/commands/reduce-order")]
    Task<ApiCommandResult?> ReduceOrder([Body] ReduceOrderRequest request);

    [Post("/queries/user-report")]
    Task<ApiCommandResult?> GetSingleUserReport([Body] UserRequest request);

    [Get("/queries/order-books/{symbolId}")]
    Task<OrderBook?> GetOrderBook(int symbolId);

    [Get("/queries/order-books")]
    Task<OrderBook[]> GetOrderBooks();

    [Get("/queries/trades")]
    Task<TradeEvent[]> GetTrades();

    [Get("/queries/orders")]
    Task<OrderDto[]> GetOrders();

    [Get("/queries/portfolios")]
    Task<PortfolioDto[]> GetPortfolios();

    [Get("/queries/market-data")]
    Task<MarketDataItemDto[]> GetMarketDataSnapshot();

    [Get("/queries/market-data/{symbolId}")]
    Task<MarketDataItemDto?> GetMarketData(int symbolId);

    [Get("/queries/market-data/{symbolId}/history")]
    Task<MarketDataItemDto[]> GetMarketDataHistory(int symbolId);

    [Post("/strategies/random-walk")]
    Task RandomWalk([Body] RandomWalkRequest request);
}

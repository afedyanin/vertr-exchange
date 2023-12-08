using Refit;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.ApiClient;

public interface ITerminalApiClient
{
    [Post("/exchange/symbols")]
    Task<ApiCommandResult?> AddSymbols([Body] AddSymbolsRequest request);

    [Post("/exchange/accounts")]
    Task<ApiCommandResult?> AddAccounts([Body] AddAccountsRequest request);

    [Post("/exchange/reset")]
    Task<ApiCommandResult?> Reset();

    [Post("/exchange/nop")]
    Task<ApiCommandResult?> Nop();

    [Post("/exchange/place-order")]
    Task<ApiCommandResult?> PlaceOrder([Body] PlaceOrderRequest request);

    [Get("/exchange/order-books/{symbolId}")]
    Task<OrderBook?> GetOrderBook(int symbolId);

    [Get("/exchange/order-books")]
    Task<OrderBook[]> GetOrderBooks();

    [Get("/exchange/trades")]
    Task<TradeItem[]> GetTrades();
}

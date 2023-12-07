using Refit;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vert.Exchange.Client.Host.ApiClient;

public interface IHostApiClient
{
    [Post("/api/exchange/symbols")]
    Task<ApiCommandResult?> AddSymbols([Body] AddSymbolsRequest request);

    [Post("/api/exchange/accounts")]
    Task<ApiCommandResult?> AddAccounts([Body] AddAccountsRequest request);

    [Post("/api/exchange/reset")]
    Task<ApiCommandResult?> Reset();

    [Post("/api/exchange/nop")]
    Task<ApiCommandResult?> Nop();

    [Post("/api/exchange/place-order")]
    Task<ApiCommandResult?> PlaceOrder([Body] PlaceOrderRequest request);
}

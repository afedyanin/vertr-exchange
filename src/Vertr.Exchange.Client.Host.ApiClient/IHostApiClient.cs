using Refit;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vert.Exchange.Client.Host.ApiClient;

public interface IHostApiClient
{
    [Post("/api/exchange/symbols")]
    Task<ApiCommandResult?> AddSymbols(AddSymbolsRequest request);

    [Post("/api/exchange/accounts")]
    Task<ApiCommandResult?> AddAccounts(AddAccountsRequest request);

    [Post("/api/exchange/reset")]
    Task<ApiCommandResult?> Reset();

    [Post("/api/exchange/nop")]
    Task<ApiCommandResult?> Nop();

    [Post("/api/exchange/place-order")]
    Task<ApiCommandResult?> PlaceOrder(PlaceOrderRequest request);
}

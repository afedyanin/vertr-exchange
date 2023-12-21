using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Terminal.ExchangeClient;

public interface IExchangeApiClient
{
    Task<ApiCommandResult> AddSymbols(AddSymbolsRequest request);

    Task<ApiCommandResult> AddAccounts(AddAccountsRequest request);

    Task<ApiCommandResult> Reset();

    Task<ApiCommandResult> Nop();

    Task<ApiCommandResult> PlaceOrder(PlaceOrderRequest request);

    Task<ApiCommandResult> CancelOrder(CancelOrderRequest request);

    Task<ApiCommandResult> MoveOrder(MoveOrderRequest request);

    Task<ApiCommandResult> ReduceOrder(ReduceOrderRequest request);
}

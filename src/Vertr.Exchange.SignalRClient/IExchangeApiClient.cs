using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Exchange.SignalRClient;

public interface IExchangeApiClient
{
    Task<ApiCommandResult> AddSymbols(AddSymbolsRequest request);

    Task<ApiCommandResult> AddAccounts(AddAccountsRequest request);

    Task<ApiCommandResult> Reset();

    Task<ApiCommandResult> Nop();

    Task<long> GetNextOrderId();

    Task<ApiCommandResult> PlaceOrder(PlaceOrderRequest request);

    Task<ApiCommandResult> CancelOrder(CancelOrderRequest request);

    Task<ApiCommandResult> MoveOrder(MoveOrderRequest request);

    Task<ApiCommandResult> ReduceOrder(ReduceOrderRequest request);

    Task<ApiCommandResult> GetSingleUserReport(UserRequest request);
}

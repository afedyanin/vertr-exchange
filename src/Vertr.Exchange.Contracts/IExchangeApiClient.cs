using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Exchange.Contracts;
public interface IExchangeApiClient
{
    long Nop();

    long GetOrderBook(OrderBookRequest orderBookRequest);

    long AddSymbols(AddSymbolsRequest addSymbolsRequest);

    long AddUser(UserRequest userRequest);

    long AddAccounts(AddAccountsRequest addAccountsRequest);

    long AdjustBalance(AdjustBalanceRequest adjustBalanceRequest);

    long CancelOrder(CancelOrderRequest cancelOrderRequest);

    long MoveOrder(MoveOrderRequest moveOrderRequest);

    long PlaceOrder(PlaceOrderRequest placeOrderRequest);

    long ReduceOrder(ReduceOrderRequest reduceOrderRequest);

    long Reset();

    long ResumeUser(UserRequest userRequest);

    long SuspendUser(UserRequest userRequest);

    long GetSingleUserReport(UserRequest userRequest);
}

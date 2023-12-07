using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Exchange.Contracts;
public interface IExchangeApiClient
{
    CommandResult Nop();

    CommandResult GetOrderBook(OrderBookRequest orderBookRequest);

    CommandResult AddSymbols(AddSymbolsRequest addSymbolsRequest);

    CommandResult AddUser(UserRequest userRequest);

    CommandResult AddAccounts(AddAccountsRequest addAccountsRequest);

    CommandResult AdjustBalance(AdjustBalanceRequest adjustBalanceRequest);

    CommandResult CancelOrder(CancelOrderRequest cancelOrderRequest);

    CommandResult MoveOrder(MoveOrderRequest moveOrderRequest);

    CommandResult PlaceOrder(PlaceOrderRequest placeOrderRequest);

    CommandResult ReduceOrder(ReduceOrderRequest reduceOrderRequest);

    CommandResult Reset();

    CommandResult ResumeUser(UserRequest userRequest);

    CommandResult SuspendUser(UserRequest userRequest);

    CommandResult GetSingleUserReport(UserRequest userRequest);
}

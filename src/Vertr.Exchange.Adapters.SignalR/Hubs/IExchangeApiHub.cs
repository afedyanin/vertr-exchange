using System.Threading.Channels;
using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Exchange.Adapters.SignalR.Hubs;

public interface IExchangeApiHub
{
    long Nop();

    long GetNextOrderId();

    long GetOrderBook(OrderBookRequest orderBookRequest);

    long AddSymbols(AddSymbolsRequest addSymbolsRequest);

    long AddUser(UserRequest userRequest);

    long AddAccounts(AddAccountsRequest addAccountsRequest);

    long AdjustBalance(AdjustBalanceRequest adjustBalanceRequest);

    long CancelOrder(CancelOrderRequest cancelOrderRequest);

    long MoveOrder(MoveOrderRequest moveOrderRequest);

    long PlaceOrder(PlaceOrderRequest placeOrderRequest, long? orderId = null);

    long ReduceOrder(ReduceOrderRequest reduceOrderRequest);

    long Reset();

    long ResumeUser(UserRequest userRequest);

    long SuspendUser(UserRequest userRequest);

    long GetSingleUserReport(UserRequest userRequest);

    ChannelReader<ApiCommandResult> ApiCommandResults();

    ChannelReader<OrderBook> OrderBooks();

    ChannelReader<ReduceEvent> ReduceEvents();

    ChannelReader<RejectEvent> RejectEvents();

    ChannelReader<TradeEvent> TradeEvents();
}

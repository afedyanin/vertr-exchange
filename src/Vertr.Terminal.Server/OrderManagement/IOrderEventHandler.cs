using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;

namespace Vertr.Terminal.Server.OrderManagement;

public interface IOrderEventHandler
{
    Task HandlePlaceOrderRequest(PlaceOrderRequest request, ApiCommandResult result);

    Task HandleMoveOrderRequest(MoveOrderRequest request, ApiCommandResult result);

    Task HandleCancelOrderRequest(CancelOrderRequest request, ApiCommandResult result);

    Task HandleReduceOrderRequest(ReduceOrderRequest request, ApiCommandResult result);

    Task HandleTradeEvent(TradeEvent tradeEvent);

    Task HandleReduceEvent(ReduceEvent reduceEvent);

    Task HandleRejectEvent(RejectEvent rejectEvent);

    Task Reset();
}

using Vertr.Exchange.Contracts;
using Vertr.Exchange.Contracts.Requests;
using Vertr.Exchange.Shared.Enums;

namespace Vertr.Terminal.Domain.OrderManagement;

public static class OrderEventFactory
{
    public static OrderEvent Create(TradeEvent taker, Trade maker)
    {
        return new OrderEvent
        {
            OrderId = maker.MakerOrderId,
            TimeStamp = taker.Timestamp,
            Seq = taker.Seq,
            Action = taker.TakerAction == OrderAction.ASK ? OrderAction.BID : OrderAction.ASK,
            OrderCompleted = maker.MakerOrderCompleted,
            Price = maker.Price,
            Volume = maker.Volume,
            EventSource = OrderEventSource.MakerTradeEvent,
        };
    }

    public static OrderEvent Create(TradeEvent taker)
    {
        return new OrderEvent
        {
            OrderId = taker.TakerOrderId,
            TimeStamp = taker.Timestamp,
            Seq = taker.Seq,
            Action = taker.TakerAction,
            OrderCompleted = taker.TakeOrderCompleted,
            Volume = taker.TotalVolume,
            EventSource = OrderEventSource.TakerTradeEvent,
        };
    }

    public static OrderEvent Create(RejectEvent rejectEvent)
    {
        return new OrderEvent
        {
            OrderId = rejectEvent.OrderId,
            TimeStamp = rejectEvent.Timestamp,
            Seq = rejectEvent.Seq,
            Price = rejectEvent.Price,
            Volume = rejectEvent.RejectedVolume,
            EventSource = OrderEventSource.RejectEvent,
        };
    }

    public static OrderEvent Create(ReduceEvent reduceEvent)
    {
        return new OrderEvent
        {
            OrderId = reduceEvent.OrderId,
            TimeStamp = reduceEvent.Timestamp,
            Seq = reduceEvent.Seq,
            Price = reduceEvent.Price,
            Volume = reduceEvent.ReducedVolume,
            EventSource = OrderEventSource.ReduceEvent,
        };
    }

    public static OrderEvent Create(MoveOrderRequest request, ApiCommandResult result)
    {

        return new OrderEvent
        {
            OrderId = request.OrderId,
            CommandResultCode = result.ResultCode,
            TimeStamp = result.Timestamp,
            Seq = result.Seq,
            EventSource = OrderEventSource.MoveOrderRequest,
            Price = request.NewPrice,
        };
    }

    public static OrderEvent Create(ReduceOrderRequest request, ApiCommandResult result)
    {
        return new OrderEvent
        {
            OrderId = request.OrderId,
            CommandResultCode = result.ResultCode,
            TimeStamp = result.Timestamp,
            Seq = result.Seq,
            EventSource = OrderEventSource.ReduceOrderRequest,
            Volume = request.ReduceSize,
        };
    }

    public static OrderEvent Create(PlaceOrderRequest request, ApiCommandResult result)
    {

        return new OrderEvent
        {
            OrderId = result.OrderId,
            CommandResultCode = result.ResultCode,
            TimeStamp = result.Timestamp,
            Seq = result.Seq,
            EventSource = OrderEventSource.PlaceOrderRequest,
            Price = request.Price,
            Action = request.Action,
            Volume = request.Size,
        };
    }

    public static OrderEvent CreateCancel(ApiCommandResult result)
    {
        return new OrderEvent
        {
            OrderId = result.OrderId,
            CommandResultCode = result.ResultCode,
            TimeStamp = result.Timestamp,
            Seq = result.Seq,
            EventSource = OrderEventSource.CacnelOrderRequest,
        };
    }
}

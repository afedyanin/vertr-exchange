using Vertr.Exchange.Domain.Abstractions;
using Vertr.Exchange.Domain.Enums;

namespace Vertr.Exchange.Domain;

internal class OrderBook : IOrderBook
{
    private readonly IDictionary<long, IOrder> _orders;

    private readonly SortedDictionary<long, OrdersBucket> _bidBuckets;
    private readonly SortedDictionary<long, OrdersBucket> _askBuckets;

    public OrderBook()
    {
        _orders = new Dictionary<long, IOrder>();

        _askBuckets = new SortedDictionary<long, OrdersBucket>();
        // TODO: Reverse sort order
        // https://stackoverflow.com/questions/931891/reverse-sorted-dictionary-in-net
        _bidBuckets = new SortedDictionary<long, OrdersBucket>();
    }

    public CommandResultCode ProcessCommand(OrderCommand cmd)
    {
        var commandType = cmd.Command;

        if (commandType == OrderCommandType.MOVE_ORDER)
        {
            return MoveOrder(cmd);
        }
        else if (commandType == OrderCommandType.CANCEL_ORDER)
        {
            return CancelOrder(cmd);
        }
        else if (commandType == OrderCommandType.REDUCE_ORDER)
        {
            return ReduceOrder(cmd);
        }
        else if (commandType == OrderCommandType.PLACE_ORDER)
        {
            if (cmd.ResultCode == CommandResultCode.VALID_FOR_MATCHING_ENGINE)
            {
                NewOrder(cmd);
                return CommandResultCode.SUCCESS;
            }
            else
            {
                return cmd.ResultCode; // no change
            }
        }
        else if (commandType == OrderCommandType.ORDER_BOOK_REQUEST)
        {
            int size = (int)cmd.Size;
            cmd.MarketData = GetL2MarketDataSnapshot(size >= 0 ? size : int.MaxValue);
            return CommandResultCode.SUCCESS;
        }
        else
        {
            return CommandResultCode.MATCHING_UNSUPPORTED_COMMAND;
        }
    }

    private void NewOrder(OrderCommand cmd)
    {
        switch (cmd.OrderType)
        {
            case OrderType.GTC:
                NewOrderPlaceGtc(cmd);
                break;
            case OrderType.IOC:
                NewOrderMatchIoc(cmd);
                break;
            case OrderType.FOK_BUDGET:
                NewOrderMatchFokBudget(cmd);
                break;
            case OrderType.IOC_BUDGET:
            // TODO: Implement IOC_BUDGET
            case OrderType.FOK:
            // TODO: Implement FOK
            default:
                //log.warn("Unsupported order type: {}", cmd);
                OrderBookEventsHelper.AttachRejectEvent(cmd, cmd.Size);
                break;
        }
    }
    private CommandResultCode CancelOrder(OrderCommand cmd)
    {
        var orderId = cmd.OrderId;

        if (!_orders.TryGetValue(orderId, out var order))
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        if (order.Uid != cmd.Uid)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        _orders.Remove(orderId);

        var buckets = GetBucketsByAction(order.Action);

        if (!buckets.TryGetValue(order.Price, out var bucket))
        {
            throw new InvalidOperationException($"Can not find bucket for OrderId={order.OrderId} Price={order.Price}");
        }

        var removed = bucket.Remove(order);

        if (!removed)
        {
            // TODO: How to handle fail removing
            throw new InvalidOperationException($"Can not remove OrderId={order.OrderId}");
        }

        if (bucket.TotalVolume == 0L)
        {
            buckets.Remove(order.Price);
        }

        cmd.MatcherEvent = OrderBookEventsHelper.CreateReduceEvent(order, order.Remaining, true);

        // fill action fields (for events handling)
        // TODO: How and where is it used?
        cmd.Action = order.Action;

        return CommandResultCode.SUCCESS;
    }

    private void NewOrderPlaceGtc(OrderCommand cmd)
    {
        throw new NotImplementedException();
    }

    private void NewOrderMatchIoc(OrderCommand cmd)
    {
        throw new NotImplementedException();
    }

    private void NewOrderMatchFokBudget(OrderCommand cmd)
    {
        throw new NotImplementedException();
    }

    private CommandResultCode ReduceOrder(OrderCommand cmd)
    {
        throw new NotImplementedException();
    }

    private CommandResultCode MoveOrder(OrderCommand cmd)
    {
        throw new NotImplementedException();
    }

    private L2MarketData GetL2MarketDataSnapshot(int size)
    {
        throw new NotImplementedException();
    }

    private SortedDictionary<long, OrdersBucket> GetBucketsByAction(OrderAction action)
        => action == OrderAction.ASK ? _askBuckets : _bidBuckets;
}

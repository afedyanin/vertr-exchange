using System;
using System.Data;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Vertr.ExchCore.Domain.Abstractions;
using Vertr.ExchCore.Domain.Enums;
using Vertr.ExchCore.Domain.ValueObjects;
using static System.Collections.Specialized.BitVector32;

namespace Vertr.ExchCore.Domain.Entities;

internal class OrderBook : IOrderBook
{
    private readonly IDictionary<long, Order> _idMap = new Dictionary<long, Order>();

    private readonly IDictionary<long, OrdersBucket> _askBuckets = new Dictionary<long, OrdersBucket>();
    private readonly IDictionary<long, OrdersBucket> _bidBuckets = new Dictionary<long, OrdersBucket>();

    private readonly OrderBookEventsHelper _eventsHelper;

    public OrderBook(
        OrderBookEventsHelper eventsHelper)
    {
        _eventsHelper = eventsHelper;
    }


    public CommandResultCode ProcessCommand(OrderCommand orderCommand)
    {
        OrderCommandType commandType = orderCommand.CommandType;

        if (commandType == OrderCommandType.MOVE_ORDER)
        {

            return MoveOrder(orderCommand);

        }
        else if (commandType == OrderCommandType.CANCEL_ORDER)
        {

            return CancelOrder(orderCommand);

        }
        else if (commandType == OrderCommandType.REDUCE_ORDER)
        {

            return ReduceOrder(orderCommand);

        }
        else if (commandType == OrderCommandType.PLACE_ORDER)
        {

            if (orderCommand.ResultCode == CommandResultCode.VALID_FOR_MATCHING_ENGINE)
            {
                NewOrder(orderCommand);
                return CommandResultCode.SUCCESS;
            }
            else
            {
                return orderCommand.ResultCode; // no change
            }

        }
        else if (commandType == OrderCommandType.ORDER_BOOK_REQUEST)
        {
            int size = (int)orderCommand.Size;
            orderCommand.L2MarketData = GetL2MarketDataSnapshot(size >= 0 ? size : int.MaxValue);
            return CommandResultCode.SUCCESS;

        }
        else
        {
            return CommandResultCode.MATCHING_UNSUPPORTED_COMMAND;
        }
    }

    public L2MarketData GetL2MarketDataSnapshot(int size)
    {
        throw new NotImplementedException();
    }

    private void NewOrder(OrderCommand orderCommand)
    {
        switch (orderCommand.OrderType)
        {
            case OrderType.GTC:
                NewOrderPlaceGtc(orderCommand);
                break;
            case OrderType.IOC:
                NewOrderMatchIoc(orderCommand);
                break;
            case OrderType.FOK_BUDGET:
                NewOrderMatchFokBudget(orderCommand);
                break;
            // TODO IOC_BUDGET and FOK support
            default:
                throw new InvalidOperationException();
                // TODO: Implement this
                //log.warn("Unsupported order type: {}", cmd);
                //eventsHelper.attachRejectEvent(cmd, cmd.size);
        }
    }

    private void NewOrderPlaceGtc(OrderCommand cmd)
    {
        var action = cmd.Action;
        var price = cmd.Price;
        var size = cmd.Size;

        // check if order is marketable (if there are opposite matching orders)
        var filledSize = TryMatchInstantly(cmd, GetSubtreeForMatching(action, price), 0, cmd);
        if (filledSize == size)
        {
            // order was matched completely - nothing to place - can just return
            return;
        }

        long newOrderId = cmd.OrderId;
        if (_idMap.ContainsKey(newOrderId))
        {
            // duplicate order id - can match, but can not place
            // TODO: eventsHelper.attachRejectEvent(cmd, cmd.size - filledSize);
            // TODO: log.warn("duplicate order id: {}", cmd);
            return;
        }

        // normally placing regular GTC limit order
        var orderRecord = new Order
        {
            OrderId = newOrderId,
            Price = price,
            Size = size,
            Filled = filledSize,
            ReserveBidPrice = cmd.ReserveBidPrice,
            Action = action,
            Uid = cmd.Uid,
            Timestamp = cmd.Timestamp,
        };

        var buckets = GetBucketsByAction(action);
        // TODO: Implement this
        // var bucket = buckets.ContainsKey(price) ? buckets[price] : buckets.Add(price, new OrderBucket()); 
        // bucket.Add(orderRecord);

        _idMap.Add(newOrderId, orderRecord);
    }

    private IDictionary<long, OrdersBucket> GetBucketsByAction(OrderAction action)
    {
        return action == OrderAction.ASK ? _askBuckets : _bidBuckets;
    }

    private IDictionary<long, OrdersBucket> GetSubtreeForMatching(OrderAction action, long price)
    {
        return action == OrderAction.ASK ? _bidBuckets : _askBuckets;
    }


    private void NewOrderMatchIoc(OrderCommand cmd)
    {
        throw new NotImplementedException();
    }

    private void NewOrderMatchFokBudget(OrderCommand cmd)
    {
        throw new NotImplementedException();
    }

    private CommandResultCode CancelOrder(OrderCommand orderCommand)
    {
        throw new NotImplementedException();
    }

    private CommandResultCode ReduceOrder(OrderCommand orderCommand)
    {
        throw new NotImplementedException();
    }

    private CommandResultCode MoveOrder(OrderCommand orderCommand)
    {
        throw new NotImplementedException();
    }

    private long TryMatchInstantly(
            IOrder activeOrder,
            IDictionary<long, OrdersBucket> matchingBuckets,
            long filled,
            OrderCommand triggerCmd)
    {

        if (matchingBuckets.Count() == 0)
        {
            return filled;
        }

        long orderSize = activeOrder.Size;

        MatcherTradeEvent? eventsTail = null;
        List<long> emptyBuckets = new List<long>();

        foreach (var bucket in matchingBuckets.Values)
        {
            var sizeLeft = orderSize - filled;

            var bucketMatchings = bucket.Match(sizeLeft, activeOrder, _eventsHelper);

            foreach(var orderId in bucketMatchings.OrdersToRemove)
            {
                _idMap.Remove(orderId);
            }

            filled += bucketMatchings.Volume;

            // attach chain received from bucket matcher
            if (eventsTail == null)
            {
                triggerCmd.MatcherEvent = bucketMatchings.EventsChainHead;
            }
            else
            {
                eventsTail.NextEvent = bucketMatchings.EventsChainHead;
            }
            eventsTail = bucketMatchings.EventsChainTail;

            long price = bucket.Price;

            // remove empty buckets
            if (bucket.TotalVolume == 0)
            {
                emptyBuckets.Add(price);
            }

            if (filled == orderSize)
            {
                // enough matched
                break;
            }
        }

        foreach(var item in emptyBuckets)
        {
            matchingBuckets.Remove(item);
        }

        return filled;
    }
}

using Vertr.Exchange.Domain.Abstractions;
using Vertr.Exchange.Domain.Enums;

namespace Vertr.Exchange.Domain;

internal class OrderBook : IOrderBook
{
    // by Key = OrderId
    private readonly IDictionary<long, IOrder> _orders;

    // by Key = Price 
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

    private void NewOrderPlaceGtc(OrderCommand cmd)
    {
        OrderAction action = cmd.Action;
        long price = cmd.Price;
        long size = cmd.Size;

        var matchingBuckets = action == OrderAction.ASK ? _bidBuckets : _askBuckets;
        long filledSize = TryMatchInstantly(cmd, matchingBuckets, 0, cmd);

        if (filledSize == size)
        {
            // order was matched completely - nothing to place - can just return
            return;
        }

        long newOrderId = cmd.OrderId;
        if (_orders.ContainsKey(newOrderId))
        {
            // duplicate order id - can match, but can not place
            OrderBookEventsHelper.AttachRejectEvent(cmd, cmd.Size - filledSize);
            //log.warn("duplicate order id: {}", cmd);
            return;
        }

        // normally placing regular GTC limit order
        var orderRecord = new Order
        {
            Action = action,
            OrderId = newOrderId,
            Price = price,
            Size = size,
            Filled = filledSize,
            Uid = cmd.Uid,
            Timestamp = cmd.Timestamp,
        };

        var buckets = GetBucketsByAction(action);

        if (!buckets.ContainsKey(price))
        {
            buckets.Add(price, new OrdersBucket(price));
        }

        var bucket = buckets[price];
        bucket.Put(orderRecord);

        _orders.Add(newOrderId, orderRecord);
    }

    private void NewOrderMatchIoc(OrderCommand cmd)
    {
        throw new NotImplementedException();
    }

    private void NewOrderMatchFokBudget(OrderCommand cmd)
    {
        throw new NotImplementedException();
    }

    private long TryMatchInstantly(
            IOrder activeOrder,
            SortedDictionary<long, OrdersBucket> matchingBuckets,
            long filled,
            OrderCommand triggerCmd)
    {
        if (!matchingBuckets.Any())
        {
            return filled;
        }

        long orderSize = activeOrder.Size;
        List<long> emptyBuckets = new List<long>();
        MatcherTradeEvent? eventsTail = null;

        foreach (var bucket in matchingBuckets.Values)
        {
            if (activeOrder.Action == OrderAction.BID && bucket.Price > activeOrder.Price)
            {
                break;
            }

            if (activeOrder.Action == OrderAction.ASK && bucket.Price < activeOrder.Price)
            {
                break;
            }

            long sizeLeft = orderSize - filled;
            MatcherResult bucketMatchings = bucket.Match(sizeLeft);

            foreach (var orderId in bucketMatchings.OrdersToRemove)
            {
                _orders.Remove(orderId);
            }

            filled += bucketMatchings.Volume;

            // attach chain received from bucket matcher
            foreach (var evt in bucketMatchings.TradeEvents)
            {
                if (eventsTail == null)
                {
                    triggerCmd.MatcherEvent = evt;
                }
                else
                {
                    eventsTail.NextEvent = evt;
                }

                eventsTail = evt;
            }

            long price = bucket.Price;

            // remove empty buckets
            if (bucket.TotalVolume == 0L)
            {
                emptyBuckets.Add(price);
            }

            if (filled == orderSize)
            {
                // enough matched
                break;
            }
        }

        foreach (var key in emptyBuckets)
        {
            matchingBuckets.Remove(key);
        }

        return filled;
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

    private CommandResultCode ReduceOrder(OrderCommand cmd)
    {
        long orderId = cmd.OrderId;
        long requestedReduceSize = cmd.Size;

        if (requestedReduceSize <= 0)
        {
            return CommandResultCode.MATCHING_REDUCE_FAILED_WRONG_SIZE;
        }

        if (!_orders.TryGetValue(orderId, out var order))
        {
            // already matched, moved or cancelled
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        if (order.Uid != cmd.Uid)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        long remainingSize = order.Remaining;
        long reduceBy = Math.Min(remainingSize, requestedReduceSize);

        var buckets = GetBucketsByAction(order.Action);

        if (!buckets.TryGetValue(order.Price, out var bucket))
        {
            throw new InvalidOperationException($"Can not find bucket for OrderId={order.OrderId} Price={order.Price}");
        }

        var canRemove = reduceBy == remainingSize;

        if (canRemove)
        {
            // now can remove order
            _orders.Remove(orderId);

            // canRemove order and whole bucket if it is empty
            bucket.Remove(order);

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
        }
        else
        {
            order.Size -= reduceBy;
            bucket.ReduceSize(reduceBy);
        }

        // send reduce event
        cmd.MatcherEvent = OrderBookEventsHelper.CreateReduceEvent(order, reduceBy, canRemove);

        // fill action fields (for events handling)
        cmd.Action = order.Action;

        return CommandResultCode.SUCCESS;
    }

    private CommandResultCode MoveOrder(OrderCommand cmd)
    {
        long orderId = cmd.OrderId;
        long newPrice = cmd.Price;

        if (!_orders.TryGetValue(orderId, out var order))
        {
            // already matched, moved or cancelled
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        if (order.Uid != cmd.Uid)
        {
            return CommandResultCode.MATCHING_UNKNOWN_ORDER_ID;
        }

        var buckets = GetBucketsByAction(order.Action);

        if (!buckets.TryGetValue(order.Price, out var bucket))
        {
            throw new InvalidOperationException($"Can not find bucket for OrderId={order.OrderId} Price={order.Price}");
        }

        // fill action fields (for events handling)
        cmd.Action = order.Action;

        // take order out of the original bucket and clean bucket if its empty
        bucket.Remove(order);

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

        order.Price = newPrice;

        // try match with new price
        var matchingBuckets = order.Action == OrderAction.ASK ? _bidBuckets : _askBuckets;
        long filled = TryMatchInstantly(order, matchingBuckets, order.Filled, cmd);

        if (filled == order.Size)
        {
            // order was fully matched (100% marketable) - removing from order book
            _orders.Remove(orderId);
            return CommandResultCode.SUCCESS;
        }

        order.Filled = filled;

        // if not filled completely - put it into corresponding bucket

        if (!buckets.ContainsKey(order.Price))
        {
            buckets.Add(order.Price, new OrdersBucket(order.Price));
        }

        var anotherBucket = buckets[order.Price];
        anotherBucket.Put(order);

        return CommandResultCode.SUCCESS;
    }
    private L2MarketData GetL2MarketDataSnapshot(int size)
    {
        int asksSize = GetTotalAskBuckets(size);
        int bidsSize = GetTotalBidBuckets(size);
        var data = new L2MarketData(asksSize, bidsSize);
        FillAsks(asksSize, data);
        FillBids(bidsSize, data);
        return data;
    }

    private void FillAsks(int size, L2MarketData data)
    {
        if (size == 0)
        {
            data.AskSize = 0;
            return;
        }

        int i = 0;

        foreach (var bucket in _askBuckets.Values)
        {
            data.AskPrices[i] = bucket.Price;
            data.AskVolumes[i] = bucket.TotalVolume;
            data.AskOrders[i] = bucket.OrdersCount;

            if (++i == size)
            {
                break;
            }
        }

        data.AskSize = i;
    }

    private void FillBids(int size, L2MarketData data)
    {
        if (size == 0)
        {
            data.BidSize = 0;
            return;
        }

        int i = 0;

        foreach (var bucket in _bidBuckets.Values)
        {
            data.BidPrices[i] = bucket.Price;
            data.BidVolumes[i] = bucket.TotalVolume;
            data.BidOrders[i] = bucket.OrdersCount;
            if (++i == size)
            {
                break;
            }
        }
        data.BidSize = i;
    }

    private int GetTotalAskBuckets(int limit)
    {
        return Math.Min(limit, _askBuckets.Count);
    }

    private int GetTotalBidBuckets(int limit)
    {
        return Math.Min(limit, _bidBuckets.Count);
    }

    private SortedDictionary<long, OrdersBucket> GetBucketsByAction(OrderAction action)
        => action == OrderAction.ASK ? _askBuckets : _bidBuckets;

    internal bool ValidateInternalState()
    {
        var asksIsValid = _askBuckets.Values.All(b => b.IsValid());
        var bidsIsValid = _bidBuckets.Values.All(b => b.IsValid());

        return asksIsValid && bidsIsValid;
    }
}

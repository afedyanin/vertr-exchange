using Vertr.ExchCore.Domain.Entities;
using Vertr.ExchCore.Domain.Enums;

namespace Vertr.ExchCore.Domain.ValueObjects;

public class OrderBookEventsHelper
{
    public MatcherTradeEvent SendTradeEvent(
        Order matchingOrder,
        bool makerCompleted,
        bool takerCompleted,
        long size,
        long bidderHoldPrice)
    {
        var @event = NewMatcherEvent();

        @event.EventType = MatcherEventType.TRADE;
        @event.Section = 0;

        @event.ActiveOrderCompleted = takerCompleted;

        @event.MatchedOrderId = matchingOrder.OrderId;
        @event.MatchedOrderUid = matchingOrder.Uid;
        @event.MatchedOrderCompleted = makerCompleted;

        @event.Price = matchingOrder.Price;
        @event.Size = size;

        // set order reserved price for correct released EBids
        @event.BidderHoldPrice = bidderHoldPrice;

        return @event;
    }

    private MatcherTradeEvent NewMatcherEvent()
    {
        /*
        if (EVENTS_POOLING)
        {
            if (eventsChainHead == null)
            {
                eventsChainHead = eventChainsSupplier.get();
                //            log.debug("UPDATED HEAD size={}", eventsChainHead == null ? 0 : eventsChainHead.getChainSize());
            }
            final MatcherTradeEvent res = eventsChainHead;
            eventsChainHead = eventsChainHead.nextEvent;
            return res;
        }
        else
        {
            return new MatcherTradeEvent();
        }
        */

        return new MatcherTradeEvent();
    }

}

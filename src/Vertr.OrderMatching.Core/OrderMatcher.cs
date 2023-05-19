using System.Runtime.CompilerServices;
using System.Diagnostics;
using Vertr.OrderMatching.Core.Books;
using Vertr.OrderMatching.Core.Trades;

[assembly: InternalsVisibleTo("Vertr.OrderMatching.Core.Tests")]

namespace Vertr.OrderMatching.Core
{
    public static class OrderMatcher
    {
        public static OrderFullfilment[] MatchBid(ref OrderBookEntry bid, OrderBook orderBook)
        {
            var trades = new List<OrderFullfilment>();

            trades.AddRange(FillOrder(ref bid, orderBook.Asks.Market));
            trades.AddRange(FillOrder(ref bid, orderBook.Asks.Limit));
            orderBook.Bids.Place(bid);

            return trades.ToArray();
        }

        public static OrderFullfilment[] MatchAsk(ref OrderBookEntry ask, OrderBook orderBook)
        {
            var trades = new List<OrderFullfilment>();

            trades.AddRange(FillOrder(ref ask, orderBook.Bids.Market));
            trades.AddRange(FillOrder(ref ask, orderBook.Bids.Limit));
            orderBook.Asks.Place(ask);

            return trades.ToArray();
        }

        internal static OrderFullfilment[] FillOrder(ref OrderBookEntry order, OrderBookSide counterSide)
        {
            if (order.IsFilled || counterSide.IsEmpty)
            {
                return Array.Empty<OrderFullfilment>();
            }

            if (order.IsMarket && counterSide.IsMarketBook)
            {
                // Cannot fill market to market
                return Array.Empty<OrderFullfilment>();
            }

            var trades = new List<OrderFullfilment>();

            while (!order.IsFilled)
            {
                if (!counterSide.Peek(out var counterOrder))
                {
                    break;
                }

                if (!CanFill(ref order, ref counterOrder))
                {
                    break;
                }

                counterOrder = counterSide.Take();
                var fullfilment = FillSingle(ref order, ref counterOrder);

                trades.Add(fullfilment);

                if (!counterOrder.IsFilled)
                {
                    Debug.Assert(order.IsFilled);
                    counterSide.Place(counterOrder);
                    break;
                }
            }

            return trades.ToArray();
        }

        internal static OrderFullfilment FillSingle(ref OrderBookEntry o1, ref OrderBookEntry o2)
        {
            var fillQty = o1.RemainingQty <= o2.RemainingQty ? o1.RemainingQty : o2.RemainingQty;

            o1 = new OrderBookEntry(
                o1.OrderId,
                o1.ArrivalTime,
                o1.Price,
                o1.RemainingQty - fillQty,
                o1.IsBid);

            o2 = new OrderBookEntry(
                o2.OrderId,
                o2.ArrivalTime,
                o2.Price,
                o2.RemainingQty - fillQty,
                o2.IsBid);

            return CreateOrderFullfilment(ref o1, ref o2, fillQty);
        }

        internal static bool CanFill(ref OrderBookEntry o1, ref OrderBookEntry o2)
        {
            if (o1.IsFilled || o2.IsFilled)
            {
                return false;
            }

            if (o1.IsMarket && o2.IsMarket)
            {
                // Cannot fill both market orders
                return false;
            }

            if (o1.IsMarket || o2.IsMarket)
            {
                // Always fill one of market order
                return true;
            }

            // Цена покупки выше или равна цене предложения
            (var bid, var ask) = BidAsk(ref o1, ref o2);
            return bid.Price >= ask.Price; 
        }

        private static OrderFullfilment CreateOrderFullfilment(ref OrderBookEntry o1, ref OrderBookEntry o2, decimal fillQty)
        {
            (var bid, var ask) = BidAsk(ref o1, ref o2);

            var fulfillment = new OrderFullfilment(
            bid.OrderId,
            ask.OrderId,
            GetTradePrice(bid.Price, ask.Price, PriceMatchingPolicy.Min),
            fillQty);

            return fulfillment;
        }

        private static (OrderBookEntry bid, OrderBookEntry ask) BidAsk(ref OrderBookEntry o1, ref OrderBookEntry o2)
        {
            Debug.Assert(o1.IsBid != o2.IsBid);
            return o1.IsBid ? (o1, o2) : (o2, o1);
        }

        private static decimal GetTradePrice(decimal bid, decimal ask, PriceMatchingPolicy priceMatchingPolicy)
        {
            if (bid == decimal.Zero)
            {
                return ask;
            }
            if (ask == decimal.Zero)
            {
                return bid;
            }

            var tradePrice = priceMatchingPolicy switch
            {
                PriceMatchingPolicy.None => decimal.Zero,
                PriceMatchingPolicy.Ask => ask,
                PriceMatchingPolicy.Bid => bid,
                PriceMatchingPolicy.Min => Math.Min(bid, ask),
                PriceMatchingPolicy.Max => Math.Max(bid, ask),
                PriceMatchingPolicy.Avg => (bid + ask) / 2.0m,
                _ => decimal.Zero
            };

            return tradePrice;
        }

        private enum PriceMatchingPolicy
        {
            None = 0,
            Bid = 1,
            Ask = 2,
            Min = 3,
            Max = 4,
            Avg = 5,
        }
    }
}

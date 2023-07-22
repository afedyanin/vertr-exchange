using Vertr.Infrastructure.Common.Contracts;
using Vertr.OrderMatching.Core;
using Vertr.OrderMatching.Core.Books;
using Vertr.OrderMatching.Domain.Contracts;
using Vertr.OrderMatching.Domain.Entities;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Domain.Services
{
    public class OrderProcessingService : IOrderProcessingService
    {
        private readonly IOrderBookRepository _orderBookRepository;
        private readonly ITradeFactory _tradeFactory;
        private readonly IOrderFulfillmentFactory _orderFulfillmentFactory;
        private readonly ITopicProvider<Trade> _tradeTopicProvider;
        private readonly ITopicProvider<OrderFulfillment> _fulfillmentTopicProvider;

        public OrderProcessingService(
            IOrderBookRepository orderBookRepository,
            ITradeFactory tradeFactory,
            IOrderFulfillmentFactory orderFulfillmentFactory,
            ITopicProvider<Trade> tradeTopicProvider,
            ITopicProvider<OrderFulfillment> fulfillmentTopicProvider)
        {
            _orderBookRepository = orderBookRepository;
            _tradeFactory = tradeFactory;
            _orderFulfillmentFactory = orderFulfillmentFactory;
            _tradeTopicProvider = tradeTopicProvider;
            _fulfillmentTopicProvider = fulfillmentTopicProvider;
        }

        public async Task Process(Order order)
        {
            var book = _orderBookRepository.GetOrAdd(order.Ticker);

            var entry = new OrderBookEntry(
                order.Id,
                order.CreationTime,
                order.Price,
                order.Qty,
                order.IsBuy);

            var matches = order.IsBuy ?
                OrderMatcher.MatchBid(ref entry, book) :
                OrderMatcher.MatchAsk(ref entry, book);

            var tradeTopic = _tradeTopicProvider.Get(order.Ticker);
            var fulfillmentTopic = _fulfillmentTopicProvider.Get(order.Ticker);

            foreach (var match in matches)
            {
                var trade = _tradeFactory.CreateTrade(
                    order.Ticker,
                    match.Price,
                    match.FilledQty);

                await tradeTopic!.Produce(trade);

                var bidOrderFulfillment = _orderFulfillmentFactory.CreateOrderFulfillment(
                    match.BuyOrderId,
                    trade.Id,
                    match.Price,
                    match.FilledQty,
                    decimal.MaxValue); // TODO: Use real remaining qty

                await fulfillmentTopic!.Produce(bidOrderFulfillment);

                var askOrderFulfillment = _orderFulfillmentFactory.CreateOrderFulfillment(
                    match.SellOrderId,
                    trade.Id,
                    match.Price,
                    match.FilledQty,
                    decimal.MaxValue); // TODO: Use real remaining qty

                await fulfillmentTopic!.Produce(askOrderFulfillment);

                // TODO: Create and send Market Data 
            }
        }
    }
}

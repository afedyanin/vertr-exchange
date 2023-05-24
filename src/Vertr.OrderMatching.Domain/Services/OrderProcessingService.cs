using Vertr.OrderMatching.Core;
using Vertr.OrderMatching.Core.Books;
using Vertr.OrderMatching.Domain.Entities;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Domain.Services
{
    public class OrderProcessingService : IOrderProcessingService
    {
        private readonly IOrderBookRepository _orderBookRepository;

        public OrderProcessingService(IOrderBookRepository orderBookRepository)
        {
            _orderBookRepository = orderBookRepository;
        }

        public Task Process(Order order)
        {
            var book = _orderBookRepository.GetOrAdd(order.Ticker);

            var entry = new OrderBookEntry(
                order.Id,
                order.CreationTime,
                order.Price,
                order.Qty,
                order.IsBuy);

            var trades = order.IsBuy ?
                OrderMatcher.MatchBid(ref entry, book) :
                OrderMatcher.MatchAsk(ref entry, book);

            /*
            foreach (var trade in trades)
            {
                //trade.???
            }
            */

            // TODO: Create and send Market Data 
            // TODO: Create and send Trades
            // TODO: Create and send OrderFullfillments

            return Task.CompletedTask;
        }
    }
}

using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Services
{
    public class OrderProcessingService : IOrderProcessingService
    {
        public Task Process(Order order)
        {
            // Get OrderBook
            // Create OrderBookEntry
            // Match Order
            // Produce Trade to Trade Topic
            // Produce Fulfillments to OrderFullfillment Topic

            return Task.CompletedTask;
        }
    }
}

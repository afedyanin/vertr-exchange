using Vertr.Common.Contracts;
using Vertr.OrderMatching.Core.Trades;

namespace Vertr.Ome.Entities
{
    public class Trade : IEntity<long>
    {
        public static Trade Empty => new();

        public long Id { get; }

        public OrderFullfilment Fulfillment { get; }

        public DateTime FulfillmentTime { get; }

        public bool IsEmpty => Id == Empty.Id;

        private Trade()
        {
        }

        internal Trade(
            long id,
            OrderFullfilment fulfillment,
            DateTime fulfillmentTime)
        {
            Id = id;
            Fulfillment = fulfillment;
            FulfillmentTime = fulfillmentTime;
        }
    }
}

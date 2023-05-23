using Vertr.OrderMatching.Domain.Contracts;

namespace Vertr.OrderMatching.Domain.Entities
{
    public class Trade : IEntity<Guid>
    {
        public static Trade Empty => new();

        public Guid Id { get; }

        public DateTime FulfillmentTime { get; }

        public bool IsEmpty => Id == Empty.Id;

        private Trade()
        {
        }

        internal Trade(
            Guid id,
            DateTime fulfillmentTime)
        {
            Id = id;
            FulfillmentTime = fulfillmentTime;
        }
    }
}

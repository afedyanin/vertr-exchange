using Vertr.Infrastructure.Common.Contracts;
using Vertr.OrderMatching.Domain.Contracts;
using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Factories
{
    public class OrderFulfillmentFactory : IOrderFulfillmentFactory
    {
        private readonly ITimeService _timeService;
        private readonly IEntityIdGenerator<Guid> _entityIdGenerator;

        public OrderFulfillmentFactory(
            ITimeService timeService,
            IEntityIdGenerator<Guid> entityIdGenerator)
        {
            _timeService = timeService;
            _entityIdGenerator = entityIdGenerator;
        }

        public OrderFulfillment CreateOrderFulfillment(
            Guid orderId,
            Guid tradeId,
            decimal price,
            decimal filledQty,
            decimal remainigQty)
        {
            var id = _entityIdGenerator.GetNextId();
            var creationTime = _timeService.GetCurrentUtcTime();

            return new OrderFulfillment(
                id,
                orderId,
                tradeId,
                price,
                filledQty,
                remainigQty,
                creationTime);
        }
    }
}

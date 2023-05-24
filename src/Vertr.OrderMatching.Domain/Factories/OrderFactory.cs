using Vertr.Infrastructure.Common.Contracts;
using Vertr.OrderMatching.Domain.Contracts;
using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Factories
{
    public class OrderFactory : IOrderFactory
    {
        private readonly ITimeService _timeService;
        private readonly IEntityIdGenerator<Guid> _entityIdGenerator;

        public OrderFactory(
            ITimeService timeService,
            IEntityIdGenerator<Guid> entityIdGenerator)
        {
            _timeService = timeService;
            _entityIdGenerator = entityIdGenerator;
        }


        public Order CreateOrder(
            Guid correlationId,
            Guid ownerId,
            string ticker,
            decimal qty,
            decimal price,
            bool isBuy)
        {
            var orderId = _entityIdGenerator.GetNextId();
            var creationTime = _timeService.GetCurrentUtcTime();

            return new Order(
                orderId,
                correlationId,
                ownerId,
                ticker,
                qty,
                price,
                isBuy,
                creationTime);
        }
    }
}

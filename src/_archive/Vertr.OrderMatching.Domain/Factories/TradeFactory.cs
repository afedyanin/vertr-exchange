using Vertr.Infrastructure.Common.Contracts;
using Vertr.OrderMatching.Domain.Contracts;
using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Factories
{
    public sealed class TradeFactory : ITradeFactory
    {
        private readonly IEntityIdGenerator<Guid> _entityIdGenerator;

        private readonly ITimeService _timeService;

        public TradeFactory(
            IEntityIdGenerator<Guid> entityIdGenerator,
            ITimeService timeService)
        {
            _entityIdGenerator = entityIdGenerator;
            _timeService = timeService;
        }

        public Trade CreateTrade(
            string ticker,
            decimal price,
            decimal qty)
        {
            var nextId = _entityIdGenerator.GetNextId();
            var currentTime = _timeService.GetCurrentUtcTime();
            var trade = new Trade(nextId, ticker, price, qty, currentTime);
            return trade;
        }
    }
}

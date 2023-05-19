using Vertr.Common.Contracts;
using Vertr.Ome.Contracts;
using Vertr.Ome.Entities;
using Vertr.OrderMatching.Core.Trades;

namespace Vertr.Ome.Factories
{
    public sealed class TradeFactory : ITradeFactory
    {
        private readonly IEntityIdGenerator<long> _entityIdGenerator;

        private readonly ITimeService _timeService;

        public TradeFactory(
            IEntityIdGenerator<long> entityIdGenerator,
            ITimeService timeService)
        {
            _entityIdGenerator = entityIdGenerator;
            _timeService = timeService;
        }

        public Trade CreateTrade(OrderFullfilment orderFullfilment)
        {
            var nextId = _entityIdGenerator.GetNextId();
            var currentTime = _timeService.GetCurrentUtcTime();
            var trade = new Trade(nextId, orderFullfilment, currentTime);
            return trade;
        }
    }
}

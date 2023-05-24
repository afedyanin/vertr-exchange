using MediatR;
using Vertr.OrderMatching.Domain.Entities;
using Vertr.OrderMatching.Domain.Repositories;

namespace Vertr.OrderMatching.Domain.Services
{
    public class TradeProcessingService : ITradeProcessingService
    {
        private readonly ITradeRepository _tradeRepository;
        private readonly IMediator _mediator;

        public TradeProcessingService(
            ITradeRepository tradeRepository,
            IMediator mediator)
        {
            _tradeRepository = tradeRepository;
            _mediator = mediator;
        }

        public Task Process(Trade trade)
        {
            var added = _tradeRepository.Insert(trade);

            if (added)
            {
                _mediator.Publish(trade); // TODO ???
            }

            return Task.CompletedTask;
        }
    }
}

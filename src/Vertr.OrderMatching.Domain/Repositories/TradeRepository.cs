using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public class TradeRepository : EntityInMemoryRepository<Trade>, ITradeRepository
    {
    }
}

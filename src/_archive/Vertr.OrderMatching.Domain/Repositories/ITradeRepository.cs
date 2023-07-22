using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Repositories
{
    public interface ITradeRepository
    {
        Trade[] GetAll();

        Trade? GetById(Guid tradeId);

        bool Insert(Trade trade);

        bool Delete(Guid tradeId);
    }
}

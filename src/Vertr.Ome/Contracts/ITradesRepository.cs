using Vertr.Ome.Entities;

namespace Vertr.Ome.Contracts
{
    public interface ITradesRepository
    {
        bool Add(Trade trade);

        bool Remove(long id);

        Trade GetById(long id);

        ICollection<Trade> GetAll();
    }
}

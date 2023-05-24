using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Contracts
{
    public interface ITradeFactory
    {
        Trade CreateTrade(
            string ticker,
            decimal price,
            decimal qty);
    }
}

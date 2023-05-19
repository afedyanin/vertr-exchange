using Vertr.Ome.Entities;
using Vertr.OrderMatching.Core.Trades;

namespace Vertr.Ome.Contracts
{
    public interface ITradeFactory
    {
        Trade CreateTrade(OrderFullfilment orderFullfilment);
    }
}

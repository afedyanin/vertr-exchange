using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Services
{
    public interface ITradeProcessingService
    {
        Task Process(Trade trade);
    }
}

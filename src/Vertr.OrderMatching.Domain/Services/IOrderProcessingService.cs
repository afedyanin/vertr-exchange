using Vertr.OrderMatching.Domain.Entities;

namespace Vertr.OrderMatching.Domain.Services
{
    public interface IOrderProcessingService
    {
        Task Process(Order order);
    }
}

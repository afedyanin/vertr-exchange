using Vertr.Terminal.Domain.PortfolioManagement;

namespace Vertr.Terminal.Domain.Abstractions;

public interface IPortfolioRepository
{
    Task<Portfolio> AddOrUpdate(Portfolio portfolio);

    Task<bool> Remove(long uid);

    Task<Portfolio[]> GetList();

    Task<Portfolio?> GetByUid(long uid);

    Task Reset();
}

using Vertr.Terminal.Domain.Abstractions;
using Vertr.Terminal.Domain.PortfolioManagement;

namespace Vertr.Terminal.DataAccess.InMemory.Repositories;

internal class PortfolioRepository : IPortfolioRepository
{
    public Task<bool> Add(Portfolio portfolio)
    {
        throw new NotImplementedException();
    }

    public Task<Portfolio?> GetByUid(long uid)
    {
        throw new NotImplementedException();
    }

    public Task<Portfolio[]> GetList()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Remove(long uid)
    {
        throw new NotImplementedException();
    }

    public Task Reset()
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(Portfolio portfolio)
    {
        throw new NotImplementedException();
    }
}

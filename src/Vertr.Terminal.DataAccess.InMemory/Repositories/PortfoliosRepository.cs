using System.Collections.Concurrent;
using Vertr.Terminal.Domain.Abstractions;
using Vertr.Terminal.Domain.PortfolioManagement;

namespace Vertr.Terminal.DataAccess.InMemory.Repositories;

internal class PortfoliosRepository : IPortfolioRepository
{
    private readonly ConcurrentDictionary<long, Portfolio> _portfolios = [];

    public Task<Portfolio> AddOrUpdate(Portfolio portfolio)
    {
        var res = _portfolios.AddOrUpdate(
            portfolio.Uid,
            portfolio,
            (uid, oldPortfolio) => portfolio);

        return Task.FromResult(res);
    }

    public Task<Portfolio?> GetByUid(long uid)
    {
        _portfolios.TryGetValue(uid, out var portfolio);

        return Task.FromResult(portfolio);
    }

    public Task<Portfolio[]> GetList()
    {
        var res = _portfolios.Values.ToArray();

        return Task.FromResult(res);
    }

    public Task<bool> Remove(long uid)
    {
        var res = _portfolios.TryRemove(uid, out var _);
        return Task.FromResult(res);
    }

    public Task Reset()
    {
        _portfolios.Clear();
        return Task.CompletedTask;
    }
}

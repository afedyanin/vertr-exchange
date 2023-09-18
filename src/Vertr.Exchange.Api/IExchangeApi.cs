using Vertr.Exchange.Common.Abstractions;

namespace Vertr.Exchange.Api;

public interface IExchangeApi : IDisposable
{
    long Execute(IApiCommand command);

    Task<IApiCommandResult> ExecuteAsync(IApiCommand command, CancellationToken token = default);
}

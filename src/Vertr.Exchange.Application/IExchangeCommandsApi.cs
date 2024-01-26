using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Application;

public interface IExchangeCommandsApi : IDisposable
{
    Guid Id { get; }

    void Send(IApiCommand command);
}

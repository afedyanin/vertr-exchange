using Vertr.Exchange.Domain.Common.Abstractions;

namespace Vertr.Exchange.Application.Core;

public interface IExchangeCoreService : IDisposable
{
    void Send(IApiCommand apiCommand);
}
